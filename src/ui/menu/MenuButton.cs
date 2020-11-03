using Godot;
using System;

public class MenuButton : Panel
{
    private bool isHovered = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    private void OnMouseEnter()
    {
        isHovered = true;
    }


    private void OnMouseExit()
    {
        isHovered = false;
    }
}
