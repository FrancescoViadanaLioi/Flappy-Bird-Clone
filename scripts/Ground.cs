using Godot;
using System;

public partial class Ground : CharacterBody2D
{
	[Export] Player player;
	float yPos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		yPos = Position.Y; 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 pos = player.Position;
		pos.Y = yPos;
		Position = pos;
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			if(collision.GetCollider().HasMethod("StopPlayer"))
			{
				collision.GetCollider().Call("StopPlayer");
			}
		}
	}
}
