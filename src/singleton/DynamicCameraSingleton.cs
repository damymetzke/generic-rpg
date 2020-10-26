using Godot;
using System;

public class DynamicCameraSingleton : Node
{
    private float followRange = 100.0f;

    [Export]
    private Vector2 targetPosition;
    [Export]
    private Vector2 finalPosition;

    [Export]
    private Rect2 cameraBounds;
    [Export]
    private Vector2 halfScreenSize;

    public void SetCameraArea(Rect2 area)
    {
        cameraBounds = area;
    }

    public void SetHalfScreenSize(Vector2 size)
    {
        halfScreenSize = size;
    }

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

        finalPosition = targetPosition;

        if (!cameraBounds.HasNoArea())
        {
            if (finalPosition.x < cameraBounds.Position.x + halfScreenSize.x)
            {
                finalPosition.x = cameraBounds.Position.x + halfScreenSize.x;
            }
            else if (finalPosition.x > cameraBounds.End.x - halfScreenSize.x)
            {
                finalPosition.x = cameraBounds.End.x - halfScreenSize.x;
            }

            if (finalPosition.y < cameraBounds.Position.y + halfScreenSize.y)
            {
                finalPosition.y = cameraBounds.Position.y + halfScreenSize.y;
            }
            else if (finalPosition.y > cameraBounds.End.y - halfScreenSize.y)
            {
                finalPosition.y = cameraBounds.End.y - halfScreenSize.y;
            }
        }
    }

    public Vector2 GetTarget()
    {
        return finalPosition;
    }
}
