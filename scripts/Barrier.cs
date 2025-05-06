using Godot;
using System;

public partial class Barrier : Node2D
{
	[Export] NinePatchRect bottomBarrierSprite;
	[Export] float groundHeight;


	bool hasSetSprite = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(hasSetSprite) return; // If the sprite has already been set, do nothing

		if(bottomBarrierSprite != null)
		{
			hasSetSprite = true;
			Vector2 newSize = bottomBarrierSprite.Size;
			newSize.Y = groundHeight - bottomBarrierSprite.GlobalPosition.Y;
			bottomBarrierSprite.Size = newSize;
		}
	}
}
