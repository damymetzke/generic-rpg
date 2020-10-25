using Godot;
using System;

public class DynamicLayerCollisionPolygon : CollisionPolygon2D
{

	[Export]
	public byte activateOn = 0;

	public void OnSwitchLayer(byte newLayer)
	{
		if (newLayer == activateOn)
		{
			this.Disabled = false;
		}
		else
		{
			this.Disabled = true;
		}
	}
	public override void _Ready()
	{
		DynamicLayerSingleton dynamicLayer = (DynamicLayerSingleton)GetNode("/root/DynamicLayerSingleton");
		dynamicLayer.BindOnSwitchLayer(this.OnSwitchLayer);

		if (dynamicLayer.GetCurrentLayer() == activateOn)
		{
			this.Disabled = false;
		}
		else
		{
			this.Disabled = true;
		}
	}
}
