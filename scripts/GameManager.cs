using Godot;
using System;
using System.IO;

public partial class GameManager : Node
{ 
	public static GameManager Instance;

	[Export] PackedScene barrierScene;
	[Export] float gapBetweenBarriers = 550f;
	[Export] Vector2 spawnHeightRange = new Vector2(150f, 550f);
	[Export] Player player;

	private float lastPlayersX = 0f;
	private float playersXTally = 0f;
	private int score = 0;
	private int highScore = 0;
	public override void _Ready()
	{
		Instance = this;
	}
	public override void _Process(double delta)
	{
		playersXTally += player.Position.X - lastPlayersX;
		lastPlayersX = player.Position.X;

		if(playersXTally >= gapBetweenBarriers)
		{
			playersXTally = 0f;
			Node2D barrier = barrierScene.Instantiate<Node2D>();
			AddChild(barrier);

			float yPos = (float)GD.RandRange(spawnHeightRange.X, spawnHeightRange.Y);
			barrier.Position = new Vector2(player.Position.X + gapBetweenBarriers, yPos);
		}

	}
}
