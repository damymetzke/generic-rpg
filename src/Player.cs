using Godot;
using System;

[Tool]
public class Player : KinematicBody2D, IDamageable
{
    public enum EInputType
    {
        MOUSE_AND_KEYBOARD,
        CONTROLLER
    }

    // Singletons and child nodes //
    private DynamicCameraSingleton dynamicCameraSingleton;
    private DebugConsole consoleSingleton;
    private AnimatedSprite animatedSprite;
    private Area2D attackArea;
    private AnimatedSprite swordSlashAnimation;

    // Exported variables //
    // Manager
    protected CustomExportManager customExportManager;

    // Movement
    private float acceleration = 4000.0f;
    private float maxSpeed = 350.0f;
    private EInputType inputType = EInputType.MOUSE_AND_KEYBOARD;

    // combat
    private uint maxHealth = 100;
    private float meleeCooldown = 1.0f;

    // etc
    private bool updateCameraPosition = true;
    private NodePath inGameUiPath;

    Player()
    {
        customExportManager = new CustomExportManager();

        customExportManager.RegisterCategory("Player");

        customExportManager.PushGroup("Movement");
        {
            customExportManager.RegisterProperty("Acceleration", Godot.Variant.Type.Real, () => acceleration, (object value) => { acceleration = (float)value; });
            customExportManager.RegisterProperty("MaxSpeed", Godot.Variant.Type.Real, () => maxSpeed, (object value) => { maxSpeed = (float)value; });
            customExportManager.RegisterPropertyEnum("InputType", Godot.Variant.Type.Int, () => inputType, (object value) => { inputType = (EInputType)(int)value; }, (Type)typeof(EInputType));
        }
        customExportManager.PopGroup();

        customExportManager.PushGroup("Combat");
        {
            customExportManager.RegisterProperty("MaxHealth", Godot.Variant.Type.Int, () => maxHealth, (object value) => { maxHealth = (uint)(int)value; });
            customExportManager.RegisterProperty("MeleeCooldown", Godot.Variant.Type.Real, () => meleeCooldown, (object value) => { meleeCooldown = (float)value; });
        }
        customExportManager.PopGroup();

        customExportManager.PushGroup("Etc");
        {
            customExportManager.RegisterProperty("UpdateCameraPosition", Godot.Variant.Type.Bool, () => updateCameraPosition, (object value) => { updateCameraPosition = (bool)value; });
            customExportManager.RegisterProperty("InGameUiPath", Godot.Variant.Type.NodePath, () => inGameUiPath, (object value) => { inGameUiPath = (NodePath)value; });
        }
        customExportManager.PopGroup();
    }

    public override bool _Set(string property, object value)
    {
        return customExportManager.SetProperty(property, value);
    }

    public override object _Get(string property)
    {
        return customExportManager.GetProperty(property);
    }

    public override Godot.Collections.Array _GetPropertyList()
    {
        base._GetPropertyList();
        return customExportManager.GetPropertyList();
    }

    // other variables //

    private float meleeCooldownProgress = 0.0f;

    private InGameUi inGameUi;

    private uint health;

    private Vector2 motion;

    private bool godmode = false;

    // logic //

    public override void _Ready()
    {
        if (Engine.EditorHint)
        {
            return;
        }
        base._Ready();

        dynamicCameraSingleton = (DynamicCameraSingleton)GetNode("/root/DynamicCameraSingleton");
        consoleSingleton = GetNode<DebugConsole>("/root/DebugConsole");
        animatedSprite = GetNode<AnimatedSprite>("CharacterSprite");
        attackArea = GetNode<Area2D>("AttackArea");
        swordSlashAnimation = GetNode<AnimatedSprite>("AttackArea/SwordSlashAnimation");

        animatedSprite.Play("idle");


        health = maxHealth;

        Node possibleInGameUi = GetNode(inGameUiPath);
        if (possibleInGameUi is InGameUi)
        {
            inGameUi = (InGameUi)possibleInGameUi;
        }

        meleeCooldownProgress = meleeCooldown;

        consoleSingleton.RegisterCommand("godmode", "Disable all player damage", OnGodmode);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Engine.EditorHint)
        {
            return;
        }
        RotateAttackArea();

        Vector2 direction = CalculateInputDirection();
        CalculateAnimation(direction);

        CalculateMovement(direction, delta);

        if (updateCameraPosition)
        {
            dynamicCameraSingleton.UpdateTarget(Position);
        }

        TryAttack(delta);

        UpdateAttackIndicator();

    }

    private void RotateAttackArea()
    {
        switch (inputType)
        {
            case EInputType.MOUSE_AND_KEYBOARD:
                attackArea.Rotation = GetAngleTo(GetGlobalMousePosition());
                break;
            case EInputType.CONTROLLER:
                Vector2 lookDirection = new Vector2(
                    Input.GetActionStrength("player_look_right") - Input.GetActionStrength("player_look_left"),
                    Input.GetActionStrength("player_look_down") - Input.GetActionStrength("player_look_up")
                ).Normalized();
                if (lookDirection.Length() == 0.0f)
                {
                    break;
                }
                attackArea.Rotation = lookDirection.Angle();
                break;
        }
    }

    private Vector2 CalculateInputDirection()
    {
        return new Vector2(
                    Input.GetActionStrength("player_right") - Input.GetActionStrength("player_left"),
                    Input.GetActionStrength("player_down") - Input.GetActionStrength("player_up")
                ).Clamped(1.0f);
    }

    private void CalculateAnimation(Vector2 direction)
    {
        float movementAngle = direction.Angle() / Mathf.Pi;

        if (direction.Length() == 0.0f)
        {
            animatedSprite.Play("idle");

        }
        else if (Mathf.Abs(movementAngle) < 0.25f)
        {
            animatedSprite.Play("walk-right");
        }
        else if (Mathf.Abs(movementAngle) > 0.75f)
        {
            animatedSprite.Play("walk-left");
        }
        else if (movementAngle > 0.0f)
        {
            animatedSprite.Play("walk-down");
        }
        else
        {
            animatedSprite.Play("walk-up");
        }

    }

    private void CalculateMovement(Vector2 direction, float delta)
    {
        float frameAcceleration = acceleration * delta;

        if (direction.Length() == 0.0f)
        {
            if (motion.Length() > frameAcceleration)
            {
                motion -= motion.Normalized() * frameAcceleration;
            }
            else
            {
                motion = new Vector2();
            }
        }
        else
        {
            motion += frameAcceleration * direction.Normalized();
            motion = motion.Clamped(maxSpeed);
        }

        MoveAndSlide(motion);
    }

    // called when sword animation is finished
    private void OnSwordslashAnimationFinished()
    {
        swordSlashAnimation.Stop();
        swordSlashAnimation.Frame = 0;
        swordSlashAnimation.Visible = false;
    }

    private void TryAttack(float delta)
    {
        meleeCooldownProgress = Mathf.Clamp(meleeCooldownProgress + delta, 0.0f, meleeCooldown);

        if (!Input.IsActionJustPressed("player_primary_attack") || meleeCooldownProgress < meleeCooldown)
        {
            return;
        }

        swordSlashAnimation.Play("default");
        swordSlashAnimation.Visible = true;

        foreach (Area2D area in attackArea.GetOverlappingAreas())
        {
            if (!(area is IDamageable))
            {
                continue;
            }

            ((IDamageable)area).ApplyDamage(new Damage(10, this));
        }

        meleeCooldownProgress = 0.0f;
    }

    private void UpdateAttackIndicator()
    {
        inGameUi.UpdateAbilityMelee(meleeCooldownProgress / meleeCooldown);
    }

    public void ApplyDamage(Damage damage)
    {
        if (godmode)
        {
            return;
        }

        if (health <= damage.amount)
        {
            health = 0;
        }
        else
        {
            health -= damage.amount;
        }

        if (inGameUi != null)
        {
            inGameUi.UpdateHealthBar(health, maxHealth);
        }
    }

    public void OnGodmode(DebugConsole console, string command, string[] arguments)
    {
        godmode = !godmode;
        if (godmode)
        {
            console.Print($"[color=#29c90a]Enabled[/color] godmode");
        }
        else
        {
            console.Print($"[color=#e73518]Disabled[/color] godmode");
        }
    }
}
