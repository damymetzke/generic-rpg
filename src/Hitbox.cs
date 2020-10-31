using Godot;
using System;

public class Hitbox : Area2D, IDamageable
{
    // Delegate: OnDamaged //
    [Signal]
    public delegate void OnDamagedSignal(uint amount);
    public delegate void OnDamaged(Damage damage);

    private OnDamaged nativeOnDamaged;

    public void BindOnDamaged(OnDamaged function)
    {
        nativeOnDamaged += function;
    }
    // Delegate end //

    public void ApplyDamage(Damage damage)
    {
        EmitSignal("OnDamagedSignal", damage.amount);
        nativeOnDamaged.Invoke(damage);
    }
}
