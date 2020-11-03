using Godot;
using System;

public class DevMenu : Control
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (!@event.IsActionPressed("toggle_dev_menu"))
        {
            return;
        }

        Visible = !Visible;
    }
}
