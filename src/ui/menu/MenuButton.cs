using Godot;
using System;

public class MenuButton : Panel
{
    private bool isHovered = false;

    [Signal]
    public delegate void OnButtonPressed();
    private OnButtonPressed onButtonPressed = () => { };

    public void registerButtonPress(OnButtonPressed function)
    {
        onButtonPressed += function;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (!(@event is InputEventMouseButton) || !isHovered)
        {
            return;
        }

        var mouseEvent = (InputEventMouseButton)@event;

        if (mouseEvent.ButtonIndex != (int)Godot.ButtonList.Left || mouseEvent.Pressed == false)
        {
            return;
        }

        onButtonPressed.Invoke();
        EmitSignal("OnButtonPressed");
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
