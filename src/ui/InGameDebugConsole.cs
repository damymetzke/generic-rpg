using Godot;
using System;

public class InGameDebugConsole : Panel
{
    private RichTextLabel history;
    private Label input;
    private DebugConsole consoleSingleton;

    private string currentInput = "";

    public override void _Ready()
    {
        base._Ready();

        history = GetNode<RichTextLabel>("History");
        input = GetNode<Label>("Input");
        consoleSingleton = GetNode<DebugConsole>("/root/DebugConsole");

        consoleSingleton.RegisterChangeHistory(OnChangeHistory);
    }

    private void OnChangeHistory(string newHistory)
    {
        history.BbcodeText = newHistory;
    }

    public void WriteCharacter(char character)
    {
        currentInput += character;
        input.Text = currentInput;
    }

    public void DeleteCharacter()
    {
        if (currentInput.Length == 0)
        {
            return;
        }
        currentInput = currentInput.Remove(currentInput.Length - 1, 1);
        input.Text = currentInput;
    }

    public void ConfirmInput()
    {
        consoleSingleton.RunCommand(currentInput);
        currentInput = "";
        input.Text = "";
    }
}
