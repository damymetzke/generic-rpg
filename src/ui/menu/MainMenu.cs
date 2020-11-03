using Godot;
using System;

public class MainMenu : Control
{
    private MenuButton startButton;

    public override void _Ready()
    {
        base._Ready();

        startButton = GetNode<MenuButton>("Margin/Buttons/MenuButtonStart");

        startButton.registerButtonPress(OnStart);
    }

    private void OnStart()
    {
        GetTree().ChangeScene("res://levels/Sandbox.tscn");
    }
}
