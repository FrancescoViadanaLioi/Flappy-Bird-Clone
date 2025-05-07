using Godot;
using System;

public partial class PointScorer : Area2D
{
	// Método que é chamado quando outro corpo sai da área de colisão do PointScorer
	public void OnBodyExit(Node2D body)
	{
		// Verifica se o corpo que saiu da área é do tipo "RigidBody2D"
		if (body.GetClass() != "RigidBody2D") return;

		// Tenta converter o corpo que saiu para um tipo Player
		Player player = (Player)body;

		// Verifica se o player não é nulo e se o jogador não colidiu (não está em estado de crash)
		if(player != null && !player.hasCrashed)
		{
			// Se o jogador estiver vivo e sair da área, incrementa a pontuação no GameManager
			GameManager.Instance.AddPoint();
		}
	}
}
