using Godot;
using System;

public class Enemy : KinematicBody2D
{
	private AnimatedSprite sprite;

	public override void _Ready()
	{
		base._Ready();

		sprite = GetNode<AnimatedSprite>("CharacterSprite");
		sprite.Play("idle");

	}
	private void OnBodyEntered(object body)
	{
		if (!(body is Player))
		{
			return;
		}

		Player player = (Player)body;

		player.ApplyDamage(new Damage(10));
	}
}


