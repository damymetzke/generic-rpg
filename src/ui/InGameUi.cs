using Godot;
using System;

public class InGameUi : Control
{
	HealthBar healthBar;

	public override void _Ready()
	{
		base._Ready();

		healthBar = (HealthBar)GetNode("HealthBar");
	}

	// Wrapper function
	public void UpdateHealthBar(float progress)
	{
		healthBar.UpdateHealthBar(progress);
	}
}
