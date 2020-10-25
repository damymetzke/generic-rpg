using Godot;
using System;

public class DynamicLayerCollisionPolygon : CollisionPolygon2D
{

    public void OnSwitchLayer(byte newLayer)
    {
        GD.Print(newLayer);
    }
    public override void _Ready()
    {
        DynamicLayerSingleton dynamicLayer = (DynamicLayerSingleton)GetNode("/root/DynamicLayerSingleton");
        dynamicLayer.BindOnSwitchLayer(this.OnSwitchLayer);

        dynamicLayer.SwitchLayer(5);
    }
}
