using Godot;

public struct Damage
{
    public Damage(uint amount, Node from = null)
    {
        this.amount = amount;
        this.from = from;
    }

    public uint amount;
    public Node from;
}