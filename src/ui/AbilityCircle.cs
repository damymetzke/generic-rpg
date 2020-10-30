using Godot;
using System;

public class AbilityCircle : Control
{
    TextureRect circleTexture;

    public override void _Ready()
    {
        base._Ready();

        circleTexture = (TextureRect)GetNode("TextureRect");
    }

    public void UpdateProgress(float progress)
    {
        if (!(circleTexture.Material is ShaderMaterial))
        {
            return;
        }
        ((ShaderMaterial)circleTexture.Material).SetShaderParam("currentTile", (1.0 - progress) * 8.0f);
    }
}
