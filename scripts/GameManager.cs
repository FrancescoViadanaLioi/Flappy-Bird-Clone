using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public partial class GameManager : Node
{
	// Instância única do GameManager (Singleton)
	public static GameManager Instance;

	// Cena da barreira (precisamos instanciar barreiras durante o jogo)
	[Export] PackedScene barrierScene;

	// Distância entre as barreiras
	[Export] float gapBetweenBarriers = 550f;

	// Intervalo de altura para o spawn das barreiras
	[Export] Vector2 spawnHeightRange = new Vector2(150f, 550f);

	// Referência ao player e a cena atual
	[Export] Player player;
	[Export] Node currentScene;

	// Referências para mostrar a pontuação atual e o recorde
	[Export] RichTextLabel currentScoreLabel;
	[Export] RichTextLabel highScoreLabel;

	// Variáveis internas
	private float lastPlayersX = 0f;   // Última posição do jogador (para calcular a movimentação)
	private float playersXTally = 0f;   // Acumulador da distância percorrida pelo jogador
	private int score = 0;              // Pontuação atual
	private List<int> highScores = new List<int>();  // Lista para armazenar os 5 melhores scores
	private string path;                // Caminho do arquivo onde os scores são salvos

	// Método chamado quando o nó é iniciado (primeira vez que a cena é carregada)
	public override void _Ready()
	{
		// Atribui a instância do GameManager
		Instance = this;

		string data;
		path = Path.Join(ProjectSettings.GlobalizePath("user://highscore.txt"));

		// Tenta carregar os scores salvos
		if (!File.Exists(path)) return;  // Se o arquivo não existir, não faz nada
		data = File.ReadAllText(path);
		try
		{
			var scoreLines = data.Split("\n");
			foreach (var line in scoreLines)
			{
				if (int.TryParse(line.Trim(), out int parsedScore))
				{
					highScores.Add(parsedScore);
				}
			}
			highScores.Sort((a, b) => b.CompareTo(a));  // Ordena os scores
			if (highScores.Count > 5)
			{
				highScores = highScores.Take(5).ToList();  // Garante que temos apenas 5 scores
			}
		}
		catch (System.Exception)
		{
			// Caso ocorra algum erro ao ler o arquivo, nada é feito
		}

		UpdateHighScoresLabel();  // Atualiza a exibição dos scores
	}

	// Método chamado a cada frame, processa a lógica do jogo
	public override void _Process(double delta)
	{
		// Acumula a distância percorrida pelo jogador no eixo X
		playersXTally += player.Position.X - lastPlayersX;
		lastPlayersX = player.Position.X;

		// Quando o jogador percorre a distância necessária, gera uma nova barreira
		if (playersXTally >= gapBetweenBarriers)
		{
			playersXTally = 0f;  // Reseta o acumulador de distância

			// Cria uma nova barreira e a adiciona à cena
			Node2D barrier = barrierScene.Instantiate<Node2D>();
			AddChild(barrier);

			// Define a altura aleatória da barreira dentro do intervalo
			float yPos = (float)GD.RandRange(spawnHeightRange.X, spawnHeightRange.Y);
			barrier.Position = new Vector2(player.Position.X + gapBetweenBarriers, yPos);  // Posiciona a barreira
		}
	}

	// Método chamado para recarregar a cena (reiniciar o jogo)
	public void Reload()
	{
		// Verifica se a pontuação atual é maior que algum dos 5 melhores scores
		if (highScores.Count < 5 || score > highScores.Last())
		{
			highScores.Add(score);
			highScores.Sort((a, b) => b.CompareTo(a));  // Ordena a lista

			if (highScores.Count > 5)
			{
				highScores.RemoveAt(highScores.Count - 1);  // Mantém apenas os 5 melhores
			}

			// Salva os 5 melhores scores no arquivo
			File.WriteAllText(path, string.Join("\n", highScores));
		}

		// Recarrega a cena atual (reinicia o jogo)
		currentScene.GetTree().ReloadCurrentScene();
	}

	// Método que adiciona um ponto à pontuação do jogador
	public void AddPoint()
	{
		score++;  // Incrementa a pontuação
		currentScoreLabel.Text = score.ToString();  // Atualiza o label com a pontuação atual
	}

	// Atualiza o label com o ranking dos 5 melhores scores
	public void UpdateHighScoresLabel()
	{
		// Limpa o texto atual do label
		highScoreLabel.Text = "";

		// Exibe os 5 melhores scores
		for (int i = 0; i < highScores.Count; i++)
		{
			highScoreLabel.Text += $"{i + 1}º: {highScores[i]}\n";
		}
	}
}
