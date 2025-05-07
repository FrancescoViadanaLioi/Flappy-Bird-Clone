using Godot;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

public partial class Player : RigidBody2D
{
	// Variáveis exportadas para poderem ser ajustadas no editor do Godot.
	[Export] float speed = 200f; // Velocidade de movimentação do jogador.
	[Export] float jumpForce = 350f; // Força de impulso do pulo.
	[Export] float gravity = 1000f; // Força da gravidade (não está sendo usada diretamente aqui, mas pode ser útil em outras situações).
	[Export] float resetTimer = 1.5f; // Tempo após a colisão para o jogo reiniciar a cena.

	// Variável que indica se o jogador colidiu com algo.
	public bool hasCrashed { get; private set; } = false;

	// Vetor para armazenar a movimentação do jogador.
	private Vector2 moveFrame = new Vector2();

	// Vetor para o impulso do pulo (somente no eixo Y).
	private Vector2 jumpImpulse = new Vector2();

	// Variável para armazenar o tempo após a colisão.
	private float timeSinceDeath = 0f;

	// Método chamado quando o nó (Player) é instanciado na cena.
	public override void _Ready()
	{
		// Inicializa a movimentação com a velocidade definida.
		moveFrame.X = speed;

		// Inicializa o impulso do pulo no eixo Y, definindo o valor negativo para que o jogador suba.
		jumpImpulse.Y = -jumpForce;
	}

	// Método chamado a cada frame (ideal para atualizações gerais).
	public override void _Process(double delta)
	{
		// Se o jogador já colidiu, aguarda o tempo de reset para reiniciar a cena.
		if (!hasCrashed) return;

		// Aumenta o tempo desde a colisão (em segundos).
		timeSinceDeath += (float)delta;

		// Se o tempo de reset já passou, recarrega a cena.
		if (timeSinceDeath >= resetTimer)
		{
			GameManager.Instance.Reload(); // Chama o método para reiniciar a cena.
			// Reseta o tempo desde a colisão.
			timeSinceDeath = 0f;
		}
	}

	// Método chamado a cada frame de física, nesse caso, movimentação e gravidade.
	public override void _PhysicsProcess(double delta)
	{
		// Só movimenta o jogador se ele não tiver colidido com nada.
		if (!hasCrashed)
		{
			moveFrame.Y = LinearVelocity.Y; // Mantém a velocidade atual no eixo Y (gravidade + movimento).
			LinearVelocity = moveFrame; // Atualiza a velocidade do jogador.
			Rotation = 0; // Impede que o jogador gire.
			AngularVelocity = 0; // Impede que o jogador tenha qualquer rotação devido ao torque (forças rotacionais).
		}
	}

	// Método que lida com entradas do jogador (teclas pressionadas).
	public override void _Input(InputEvent @event)
	{
		// Se o jogador já colidiu, não aceita mais entradas.
		if (hasCrashed) return;

		// Aqui verificamos se o jogador pressionou uma tecla de movimento (pulo).
		if (@event is InputEventKey keyEvent && keyEvent.IsPressed())
		{
			var code = keyEvent.PhysicalKeycode; // Obtém o código da tecla pressionada.
			// Se a tecla for 'Space' ou 'Up' (espaço ou seta para cima), o jogador vai pular.
			if (code == Key.Space || code == Key.Up)
			{
				Jump(); // Chama o método de pulo.
			}
		}
	}

	// Método que faz o jogador pular.
	private void Jump()
	{
		// Zera a velocidade do jogador no eixo Y (impede que ele continue caindo durante o pulo).
		moveFrame.Y = 0f;

		// Atualiza a velocidade do jogador para continuar o movimento (com a velocidade em Y zerada).
		LinearVelocity = moveFrame;

		// Aplica um impulso para o jogador subir.
		ApplyImpulse(jumpImpulse);
	}

	// Método chamado quando o jogador colide com algo (por exemplo, um obstáculo).
	public void StopPlayer()
	{
		// Marca que o jogador colidiu, evitando que ele continue se movendo.
		hasCrashed = true;

		// Zera a velocidade do jogador (ele para de se mover).
		LinearVelocity = Vector2.Zero;

		// Desativa a gravidade para que o jogador flutue após a colisão (sem ser puxado para baixo).
		GravityScale = 0f;

		// Bloqueia a rotação do jogador (impede que ele gire após a colisão).
		LockRotation = true;
	}

	// Método chamado quando um corpo (por exemplo, um obstáculo) entra em colisão com o jogador.
	public void OnBodyEntered(Node body)
	{
		// Se o jogador já colidiu, não faz nada.
		if (hasCrashed) return;

		// Marca que o jogador colidiu com algo e, portanto, não pode mais se mover.
		hasCrashed = true;
	}
}
