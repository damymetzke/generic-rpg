using Godot;
using System;

public class DynamicCameraSingleton : Node
{
    private Vector2 targetPosition;

    public void UpdateTarget(Vector2 desiredPosition)
    {
        targetPosition = desiredPosition;
    }

    public Vector2 GetTarget()
    {
        return targetPosition;
    }
}
