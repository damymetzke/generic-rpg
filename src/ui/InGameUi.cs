using Godot;
using System;

public class InGameUi : Control
{
    HealthBar healthBar;
    AbilityCircle abilityMelee;
    private InGameDebugConsole debugConsole;
    private GlobalState globalState;

    private bool debugActive = false;

    public override void _Ready()
    {
        base._Ready();

        healthBar = (HealthBar)GetNode("HealthBar");
        abilityMelee = (AbilityCircle)GetNode("AbilityMelee");
        debugConsole = GetNode<InGameDebugConsole>("DebugConsole");
        globalState = GetNode<GlobalState>("/root/GlobalState");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (
            globalState.inputState.Get() != GlobalState.EInputState.DEBUG_CONSOLE
            && globalState.inputState.Get() != GlobalState.EInputState.GAMEPLAY
            )
        {
            return;
        }

        if (Input.IsActionJustPressed("toggle_dev_menu"))
        {
            debugActive = !debugActive;
            debugConsole.Visible = debugActive;

            if (debugActive)
            {
                globalState.inputState.Set(GlobalState.EInputState.DEBUG_CONSOLE);
            }
            else
            {
                globalState.inputState.Set(GlobalState.EInputState.GAMEPLAY);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        // only listen for keyoard input when debug console is open.
        if (!(@event is InputEventKey) || globalState.inputState.Get() != GlobalState.EInputState.DEBUG_CONSOLE)
        {
            return;
        }

        var keyEvent = (InputEventKey)@event;

        if (keyEvent.Scancode == (uint)KeyList.Backspace && keyEvent.Pressed)
        {
            debugConsole.DeleteCharacter();
            return;
        }

        if (keyEvent.Scancode == (uint)KeyList.Enter && keyEvent.Pressed)
        {
            debugConsole.ConfirmInput();
            return;
        }

        if (keyEvent.Unicode == 0)
        {
            return;
        }
        debugConsole.WriteCharacter((char)keyEvent.Unicode);
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
