using Godot;
using System;

public class Player : RigidBody2D
{
	[Export]
	private float movementSpeed = 20.0f;
	public override void _Process(float delta)
	{
		Vector2 direction = new Vector2(0.0f, 0.0f);

		if (Input.IsActionPressed("player_left"))
		{
			direction += new Vector2(-1.0f, 0.0f);
		}
		if (Input.IsActionPressed("player_right"))
		{
			direction += new Vector2(1.0f, 0.0f);
		}
		if (Input.IsActionPressed("player_up"))
		{
			direction += new Vector2(0.0f, -1.0f);
		}
		if (Input.IsActionPressed("player_down"))
		{
			direction += new Vector2(0.0f, 1.0f);
		}


		this.LinearVelocity = delta * direction * 1000.0f * this.movementSpeed;
		this.Rotation = 0.0f;
	}
}
