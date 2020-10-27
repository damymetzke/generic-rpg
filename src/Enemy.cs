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

}
