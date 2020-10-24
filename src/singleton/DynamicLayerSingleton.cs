using Godot;
using System;

public class DynamicLayerSingleton : Node
{
	private byte currentLayer = 0;

	[Signal]
	public delegate void OnSwitchLayer(byte newLayer);

	public void SwitchLayer(byte newLayer)
	{
		this.currentLayer = newLayer;
		EmitSignal(nameof(OnSwitchLayer));
	}

	public byte GetCurrentLayer()
	{
		return this.currentLayer;
	}
}
