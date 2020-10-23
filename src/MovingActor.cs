using Godot;
using System;

// Just moves right at a constant speed.
public class MovingActor : Node2D
{
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        this.SetPosition(this.GetPosition() + (delta * new Vector2(50, 0)));
    }
}
