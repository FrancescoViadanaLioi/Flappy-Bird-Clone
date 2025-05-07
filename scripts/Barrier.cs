using Godot;
using System;

public partial class Barrier : Node2D
{
	// Exporta a referência do sprite da barreira inferior.
	// Isso permite que o sprite da barreira seja atribuído no editor do Godot.
	[Export] NinePatchRect bottomBarrierSprite;

	// Exporta a altura do chão (em Y), que será usada para ajustar o tamanho da barreira.
	[Export] float groundHeight;

	// Variável que garante que o sprite será configurado apenas uma vez.
	bool hasSetSprite = false;

	// Método chamado a cada frame. 'delta' é o tempo que passou desde o último frame.
	public override void _Process(double delta)
	{
		// Se o sprite já foi configurado, não faz nada
		if (hasSetSprite) return;

		// Verifica se a referência do sprite foi atribuída
		if (bottomBarrierSprite != null)
		{
			// Marca que o sprite foi configurado para não repetir a operação
			hasSetSprite = true;

			// Obtém o tamanho atual do sprite
			Vector2 newSize = bottomBarrierSprite.Size;

			// Ajusta a altura do sprite da barreira para que ele se estenda até a altura do chão (groundHeight)
			newSize.Y = groundHeight - bottomBarrierSprite.GlobalPosition.Y;

			// Define o novo tamanho para o sprite da barreira
			bottomBarrierSprite.Size = newSize;
		}
	}
}
