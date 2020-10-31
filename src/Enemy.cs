using Godot;
using System;

public class Enemy : KinematicBody2D, IDamageable
{
    private AnimatedSprite sprite;
    private Hitbox hitbox;

    [Export]
    private float agroRadius = 300.0f;
    [Export]
    private float followRadius = 400.0f;
    [Export]
    private float acceleration = 4000.0f;
    [Export]
    private float maxSpeed = 250.0f;
    [Export]
    private float knockbackAmount = 500.0f;

    Player followingPlayer = null;

    private uint health = 25;

    private Vector2 motion;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<AnimatedSprite>("CharacterSprite");
        hitbox = GetNode<Hitbox>("Hitbox");

        sprite.Play("idle");

        hitbox.BindOnDamaged(ApplyDamage);

    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (followingPlayer == null)
        {
            Godot.Collections.Array players = GetTree().GetNodesInGroup("player");
            Player closestPlayer = null;
            foreach (object player in players)
            {
                if (!(player is Player))
                {
                    continue;
                }

                if (closestPlayer == null)
                {
                    closestPlayer = (Player)player;
                    continue;
                }

                if (
                    (Position - ((Player)player).Position).Length()
                    <
                    (Position - closestPlayer.Position).Length()
                )
                {
                    closestPlayer = (Player)player;
                }
            }

            if (closestPlayer != null && Position.DistanceTo(closestPlayer.Position) <= agroRadius)
            {
                followingPlayer = (Player)closestPlayer;
            }
        }
        else
        {
            if (Position.DistanceTo(followingPlayer.Position) >= followRadius)
            {
                followingPlayer = null;
            }
        }

        if (followingPlayer == null)
        {
            return;
        }

        MoveTowardPlayer(delta);
    }

    private void MoveTowardPlayer(float delta)
    {
        Vector2 direction = Position.DirectionTo(followingPlayer.Position);

        float frameAcceleration = acceleration * delta;

        if (direction.Length() == 0.0f)
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
            motion = motion.Clamped(maxSpeed);
        }

        MoveAndSlide(motion);
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
        if (health < damage.amount)
        {
            health = 0;
            Die();
        }

        health -= damage.amount;

        if (damage.from != null && damage.from is Node2D)
        {
            Vector2 damageDirection = ((Node2D)damage.from).Position.DirectionTo(Position);

            motion = damageDirection * knockbackAmount;
        }
    }

    private void Die()
    {
        GetParent().RemoveChild(this);
    }
}


