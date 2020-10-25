using Godot;
using System;

public class LayerSwitchTrigger : Area2D
{
	[Export]
	public byte switchLayerTo = 0;

	DynamicLayerSingleton dynamicLayer;

	public void OnBodyEntered(Node body)
	{
		dynamicLayer.SwitchLayer(switchLayerTo);
	}

	public override void _Ready()
	{
		this.Connect("body_entered", this, nameof(OnBodyEntered));
		dynamicLayer = (DynamicLayerSingleton)GetNode("/root/DynamicLayerSingleton");
	}
}
