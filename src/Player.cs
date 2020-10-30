using Godot;
using System;

public class Player : KinematicBody2D, IDamageable
{
    DynamicCameraSingleton dynamicCameraSingleton;
    private AnimatedSprite animatedSprite;

    [Export]
    private float acceleration = 4000.0f;
    [Export]
    private float maxSpeed = 350.0f;

    [Export]
    private bool updateCameraPosition = true;

    [Export]
    private uint maxHealth = 100;

    [Export]
    private NodePath inGameUiPath;

    private InGameUi inGameUi;

    private uint health;

    private Vector2 motion;

    public override void _Ready()
    {
        base._Ready();
        animatedSprite = GetNode<AnimatedSprite>("CharacterSprite");
        animatedSprite.Play("idle");

        dynamicCameraSingleton = (DynamicCameraSingleton)GetNode("/root/DynamicCameraSingleton");

        health = maxHealth;

        Node possibleInGameUi = GetNode(inGameUiPath);
        if (possibleInGameUi is InGameUi)
        {
            inGameUi = (InGameUi)possibleInGameUi;
        }

    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = CalculateInputDirection();
        CalculateAnimation(direction);

        CalculateMovement(direction, delta);

        if (updateCameraPosition)
        {
            dynamicCameraSingleton.UpdateTarget(Position);
        }

        TryAttack();

    }

    private Vector2 CalculateInputDirection()
    {
        return new Vector2(
                    Input.GetActionStrength("player_right") - Input.GetActionStrength("player_left"),
                    Input.GetActionStrength("player_down") - Input.GetActionStrength("player_up")
                ).Clamped(1.0f);
    }

    private void CalculateAnimation(Vector2 direction)
    {
        float movementAngle = direction.Angle() / Mathf.Pi;

        if (direction.Length() == 0.0f)
        {
            animatedSprite.Play("idle");

        }
        else if (Mathf.Abs(movementAngle) < 0.25f)
        {
            animatedSprite.Play("walk-right");
        }
        else if (Mathf.Abs(movementAngle) > 0.75f)
        {
            animatedSprite.Play("walk-left");
        }
        else if (movementAngle > 0.0f)
        {
            animatedSprite.Play("walk-down");
        }
        else
        {
            animatedSprite.Play("walk-up");
        }

    }

    private void CalculateMovement(Vector2 direction, float delta)
    {
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

    private void TryAttack()
    {
        if (!Input.IsActionJustPressed("player_primary_attack"))
        {
            return;
        }

        GD.Print("ATTACK");
    }

    public void ApplyDamage(Damage damage)
    {
        if (health <= damage.amount)
        {
            health = 0;
        }
        else
        {
            health -= damage.amount;
        }

        if (inGameUi != null)
        {
            inGameUi.UpdateHealthBar(health, maxHealth);
        }
    }
}
