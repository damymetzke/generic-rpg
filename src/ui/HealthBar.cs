using Godot;
using System;

public class HealthBar : VBoxContainer
{
    private TextureRect functionalBar;
    private Label text;

    public override void _Ready()
    {
        base._Ready();

        functionalBar = (TextureRect)GetNode("Bar/FunctionalBar");
        text = (Label)GetNode("Text");
    }

    public void UpdateHealthBar(uint currentHealth, uint maxHealth)
    {
        float progress = (float)currentHealth / (float)maxHealth;

        if (!(functionalBar.Material is ShaderMaterial))
        {
            return;
        }

        ((ShaderMaterial)functionalBar.Material).SetShaderParam("progress", Mathf.Clamp(progress, 0.0f, 1.0f));

        text.Text = $"HP: {currentHealth}/{maxHealth}";
    }

}
