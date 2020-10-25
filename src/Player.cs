using Godot;
using System;

public class Player : KinematicBody2D
{
	private AnimatedSprite animatedSprite;

	private enum HorizontalDirection
	{
		LEFT,
		NONE,
		RIGHT
	}
	private enum VerticalDirection
	{
		UP,
		NONE,
		DOWN
	}

	[Export]
	private float movementSpeed = 20.0f;

	private Vector2 motion;

	public override void _Ready()
	{
		base._Ready();
		animatedSprite = GetNode<AnimatedSprite>("CharacterSprite");
		animatedSprite.Play("idle");
	}

	public override void _PhysicsProcess(float delta)
	{

		bool walkingLeft = Input.IsActionPressed("player_left");
		bool walkingRight = Input.IsActionPressed("player_right");

		bool walkingUp = Input.IsActionPressed("player_up");
		bool walkingDown = Input.IsActionPressed("player_down");

		HorizontalDirection horizontalDirection = (walkingLeft == walkingRight)
			? HorizontalDirection.NONE
			: (walkingLeft) ? HorizontalDirection.LEFT : HorizontalDirection.RIGHT;

		VerticalDirection verticalDirection = (walkingUp == walkingDown)
			? VerticalDirection.NONE
			: (walkingUp) ? VerticalDirection.UP : VerticalDirection.DOWN;

		Vector2 direction = new Vector2(0.0f, 0.0f);

		switch (horizontalDirection)
		{
			case HorizontalDirection.LEFT:
				direction.x = -1.0f;
				break;
			case HorizontalDirection.NONE:
				direction.x = 0.0f;
				break;
			case HorizontalDirection.RIGHT:
				direction.x = 1.0f;
				break;
		}

		switch (verticalDirection)
		{
			case VerticalDirection.UP:
				direction.y = -1.0f;
				break;
			case VerticalDirection.NONE:
				direction.y = 0.0f;
				break;
			case VerticalDirection.DOWN:
				direction.y = 1.0f;
				break;
		}

		if (horizontalDirection == HorizontalDirection.RIGHT)
		{
			animatedSprite.Play("walk-right");
		}
		else if (horizontalDirection == HorizontalDirection.LEFT)
		{
			animatedSprite.Play("walk-left");
		}
		else if (verticalDirection == VerticalDirection.DOWN)
		{
			animatedSprite.Play("walk-down");
		}
		else if (verticalDirection == VerticalDirection.UP)
		{
			animatedSprite.Play("walk-up");
		}
		else
		{
			animatedSprite.Play("idle");
		}

		float frameAcceleration = movementSpeed * 100.0f * delta;

		if (
			horizontalDirection == HorizontalDirection.NONE
			&& verticalDirection == VerticalDirection.NONE
		)
		{
			if (motion.Length() > frameAcceleration)
			{
				motion -= motion.Normalized() * frameAcceleration;
			}
			else
			{
				motion = new Vector2();
			}
		}
		else
		{
			motion += frameAcceleration * direction.Normalized();
			motion = motion.Clamped(movementSpeed * 10.0f);
		}

		motion = MoveAndSlide(motion);

		// this.LinearVelocity = delta * direction * 1000.0f * this.movementSpeed;
		// this.Rotation = 0.0f;
	}
}
