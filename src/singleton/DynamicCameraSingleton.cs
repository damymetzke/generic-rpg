using Godot;
using System;

public class DynamicCameraSingleton : Node
{
    private float followRange = 130.0f;

    private Vector2 targetPosition;

    public void UpdateTarget(Vector2 desiredPosition)
    {
        Vector2 nextTargetPosition = targetPosition;
        if (Mathf.Abs(targetPosition.x - desiredPosition.x) >= followRange)
        {
            nextTargetPosition.x = desiredPosition.x + followRange * Mathf.Sign(targetPosition.x - desiredPosition.x);
        }
        if (Mathf.Abs(targetPosition.y - desiredPosition.y) >= followRange)
        {
            nextTargetPosition.y = desiredPosition.y + followRange * Mathf.Sign(targetPosition.y - desiredPosition.y);
        }
        targetPosition = nextTargetPosition;
    }

    public Vector2 GetTarget()
    {
        return targetPosition;
    }
}
