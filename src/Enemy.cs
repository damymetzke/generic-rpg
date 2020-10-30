using Godot;
using System;

public class Enemy : KinematicBody2D, IDamageable
{
    private AnimatedSprite sprite;
    private Hitbox hitbox;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<AnimatedSprite>("CharacterSprite");
        hitbox = GetNode<Hitbox>("Hitbox");

        sprite.Play("idle");

        hitbox.BindOnDamaged(ApplyDamage);

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

    public void ApplyDamage(Damage damage)
    {
        GD.Print("ouch");
    }
}


