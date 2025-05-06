using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : RigidBody2D
{
	[Export] float speed = 200f;

	[Export] float jumpForce = 350f;

	[Export] float gravity = 1000f;

	//Quando o jogador colide, esse será o tempo até o reset
	[Export] float resetTimer = 1.5f;

	private Vector2 moveFrame = new Vector2();

	private Vector2 jumpImpulse = new Vector2();
	private bool hasCrashed = false;

	public override void _Ready()
	{
		moveFrame.X = speed; 
		jumpImpulse.Y = -jumpForce;
	}

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// Só movimenta o player se não tiver colidido
		if (!hasCrashed)
		{
			moveFrame.Y = LinearVelocity.Y;
			LinearVelocity = moveFrame;
			Rotation = 0; // Impede que o player gire
			AngularVelocity = 0; // Impede qualquer rotação devido ao torque
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (hasCrashed) return;

		// Aqui é quando o jogador pressiona a tecla de movimento, a velocidade de queda é zerada, e o jogador começa a subir
		if(@event is InputEventKey keyEvent && keyEvent.IsPressed())
		{
			var code = keyEvent.PhysicalKeycode;
			// Verifica se o jogador está pressionando a tecla de movimento
			if (code == Key.Space || code == Key.Up)
			{
				Jump();
			}
		}
	}

	private void Jump()
	{
		// Zerando a velocidade de queda
		moveFrame.Y = 0f;
		// Adicionando a velocidade de subida
		LinearVelocity = moveFrame;
		ApplyImpulse(jumpImpulse);
	}

	// Método que verifica se o jogador teve colisão
	public void StopPlayer()
	{
		hasCrashed = true;
		LinearVelocity = Vector2.Zero;  // Para o movimento
		GravityScale = 0f;  // Desliga a gravidade
		LockRotation = true;  // Bloqueia a rotação
	}
}
