using Godot;
using System;

public class DebugConsole : Node
{
    public delegate void OnChangeHistory(string newHistory);
    private OnChangeHistory onChangeHistory = (string newHistory) => { };

    public delegate void OnCommand(DebugConsole console, string command, string[] arguments);

    private System.Collections.Generic.Dictionary<string, OnCommand> commands = new System.Collections.Generic.Dictionary<string, OnCommand>();

    public void RegisterChangeHistory(OnChangeHistory history)
    {
        onChangeHistory += history;
    }

    public void RunCommand(string command)
    {
        history += $"\n> [color=#bbbbbb]{command}[/color]";

        string[] split = command.Split(" ");
        string[] arguments = new string[split.Length - 1];
        for (uint i = 1; i < split.Length; ++i)
        {
            arguments[i - 1] = split[i];
        }
        if (split.Length > 0 && commands.ContainsKey(split[0]))
        {
            commands[split[0]](this, split[0], arguments);
        }

        onChangeHistory.Invoke(history);
    }

    public void Print(string text, bool updateHistory = false)
    {
        history += $"\n{text}";
        if (!updateHistory)
        {
            return;
        }

        onChangeHistory.Invoke(history);
    }

    public void RegisterCommand(string commandName, string description, OnCommand callback)
    {
        commands.Add(commandName, callback);
    }

    private string history = "";
}