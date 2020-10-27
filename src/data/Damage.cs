using Godot;

struct Damage
{
    public Damage(uint amount, Node from = null)
    {
        this.amount = amount;
        this.from = from;
    }

    uint amount;
    Node from;
}