using Godot;
using System;

public class DynamicLayerSingleton : Node
{
	private byte currentLayer = 0;

	[Signal]
	public delegate void OnSwitchLayer(byte newLayer);

	private OnSwitchLayer nativeOnSwitchLayer;

	public void BindOnSwitchLayer(OnSwitchLayer function)
	{
		nativeOnSwitchLayer += function;
	}

	public void SwitchLayer(byte newLayer)
	{
		this.currentLayer = newLayer;
		EmitSignal(nameof(OnSwitchLayer));
		nativeOnSwitchLayer.Invoke(newLayer);
	}

	public byte GetCurrentLayer()
	{
		return this.currentLayer;
	}
}
