using Godot;
using System;

public class InGameUi : Control
{
    HealthBar healthBar;
    AbilityCircle abilityMelee;

    public override void _Ready()
    {
        base._Ready();

        healthBar = (HealthBar)GetNode("HealthBar");
        abilityMelee = (AbilityCircle)GetNode("AbilityMelee");
    }

    // Wrapper functions
    public void UpdateHealthBar(uint currentHealth, uint maxHealth)
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void UpdateAbilityMelee(float progress)
    {
        abilityMelee.UpdateProgress(progress);
    }
}
