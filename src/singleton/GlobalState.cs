using Godot;

class GlobalState : Node
{
    public enum EInputState
    {
        GAMEPLAY,
        DEBUG_CONSOLE,
        MENU
    }

    private EInputState inputState;

    public EInputState GlobalInputState
    {
        get { return inputState; }
        set { inputState = value; }
    }
}