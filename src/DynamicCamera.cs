using Godot;
using System;

public class DynamicCamera : Camera2D
{
	DynamicCameraSingleton dynamicCameraSingleton;

	public override void _Ready()
	{
		base._Ready();
		dynamicCameraSingleton = (DynamicCameraSingleton)GetNode("/root/DynamicCameraSingleton");

		dynamicCameraSingleton.SetHalfScreenSize(GetViewportRect().Size * 0.5f);
	}

	public override void _Process(float delta)
	{
		base._Process(delta);

		GlobalPosition = dynamicCameraSingleton.GetTarget();
	}
}
