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
        // dynamicLayer.Connect(nameof(DynamicLayerSingleton.OnSwitchLayer), this, nameof(OnSwitchLayer));

        GD.Print(this.HasMethod(nameof(OnSwitchLayer)));

        dynamicLayer.SwitchLayer(5);
    }
}
