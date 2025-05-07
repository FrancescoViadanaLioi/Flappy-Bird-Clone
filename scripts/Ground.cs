using Godot;
using System;

public partial class Ground : CharacterBody2D
{
	// Exporta a referência do jogador, para que possamos definir isso no editor do Godot.
	[Export] Player player;

	// Variável para armazenar a posição inicial do chão (Y).
	float yPos;

	// Método chamado quando o nó (Ground) é adicionado à cena pela primeira vez.
	public override void _Ready()
	{
		// Armazena a posição Y atual do chão. 
		// Isso é feito para manter a posição Y constante enquanto movemos o chão no eixo X.
		yPos = Position.Y;
	}

	// Método chamado a cada frame. 'delta' é o tempo que passou desde o último frame.
	public override void _Process(double delta)
	{
		// Obtém a posição do jogador.
		Vector2 pos = player.Position;

		// Define a posição Y do chão para ser igual à posição Y do chão inicial (yPos).
		// Isso garante que o chão se mova na mesma altura, mantendo a posição do jogador.
		pos.Y = yPos;

		// Atualiza a posição do chão com a nova posição.
		Position = pos;

		// Move o chão de acordo com a física.
		// O método MoveAndSlide() é usado para garantir que o movimento do chão leve em consideração
		// colisões e deslizamentos com outros objetos (por exemplo, o jogador ou obstáculos).
		MoveAndSlide();

		// Verifica as colisões do chão.
		// 'GetSlideCollisionCount()' retorna quantas colisões ocorreram enquanto o chão se moveu.
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			// Obtemos as colisões usando o método 'GetSlideCollision()'.
			KinematicCollision2D collision = GetSlideCollision(i);

			// Verificamos se o objeto colidido possui o método "StopPlayer".
			// Se tiver, significa que o objeto é o jogador (Player), e então chamamos o método "StopPlayer" no jogador.
			if (collision.GetCollider().HasMethod("StopPlayer"))
			{
				// Chama o método "StopPlayer" do jogador, o que vai interromper o movimento do jogador.
				collision.GetCollider().Call("StopPlayer");
			}
		}
	}
}
