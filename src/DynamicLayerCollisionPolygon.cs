using Godot;
using System;

public class DynamicLayerCollisionPolygon : StaticBody2D
{

    [Export]
    public byte activateOn = 0;

    public void OnSwitchLayer(byte newLayer)
    {
        if (newLayer == activateOn)
        {
            SetCollisionLayerBit(0, true);
        }
        else
        {
            SetCollisionLayerBit(0, false);
        }
    }
    public override void _Ready()
    {
        DynamicLayerSingleton dynamicLayer = (DynamicLayerSingleton)GetNode("/root/DynamicLayerSingleton");
        dynamicLayer.BindOnSwitchLayer(this.OnSwitchLayer);

        if (dynamicLayer.GetCurrentLayer() == activateOn)
        {
            SetCollisionLayerBit(0, true);
        }
        else
        {
            SetCollisionLayerBit(0, false);
        }
    }
}
