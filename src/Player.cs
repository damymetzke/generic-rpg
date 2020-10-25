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
    private float acceleration = 4000.0f;

    [Export]
    private float maxSpeed = 350.0f;


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

        Vector2 direction = new Vector2(
            Input.GetActionStrength("player_right") - Input.GetActionStrength("player_left"),
            Input.GetActionStrength("player_down") - Input.GetActionStrength("player_up")
        );

        if (direction.Length() >= 1.0f)
        {
            direction = direction.Normalized();
        }

        float frameAcceleration = acceleration * delta;

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
            motion = motion.Clamped(maxSpeed);
        }

        motion = MoveAndSlide(motion);
    }
}
