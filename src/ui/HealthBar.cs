using Godot;
using System;

public class HealthBar : TextureRect
{
	public void UpdateHealthBar(float progress)
	{
		if (!(Material is ShaderMaterial))
		{
			return;
		}

		((ShaderMaterial)Material).SetShaderParam("progress", Mathf.Clamp(progress, 0.0f, 1.0f));
	}

	private float timerProgress = 0.0f;

	public override void _Process(float delta)
	{
		timerProgress = (timerProgress + delta) % 1.0f;
		UpdateHealthBar(timerProgress);
	}

}
