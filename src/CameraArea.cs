using Godot;
using System;

public class CameraArea : Area2D
{
	private Rect2 area;

	public override void _Ready()
	{
		base._Ready();

		CollisionShape2D areaShape = GetNode<CollisionShape2D>("AreaShape");

		Vector2 areaShapeExtents = ((RectangleShape2D)areaShape.Shape).Extents;
		Vector2 areaShapeOrigin = areaShape.Position;

		area = new Rect2(
			areaShapeOrigin - areaShapeExtents,
			2.0f * areaShapeExtents
		);

		DynamicCameraSingleton dynamicCameraSingleton = (DynamicCameraSingleton)GetNode("/root/DynamicCameraSingleton");
		dynamicCameraSingleton.SetCameraArea(area);

	}
}
