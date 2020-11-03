using Godot;
using System;


class DebugConsole : Node
{
    public delegate void OnChangeHistory(string newHistory);
    private OnChangeHistory onChangeHistory = (string newHistory) => { };

    public void RegisterChangeHistory(OnChangeHistory history)
    {
        onChangeHistory += history;
    }

    public void RunCommand(string command)
    {
        history += $"\n> [color=#bbbbbb]{command}[/color]";

        onChangeHistory.Invoke(history);
    }

    private string history = "";
}