using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieBounceRide : CharacterState
{
    [ExportCategory("Nodes")]
    [Export] public AnimationPlayer AnimationPlayer;
    [Export] public Sprite2D Shadow;
    [Export] public AnimatedSprite2D Dust;
    [ExportCategory("Constants")]
    [Export] public double HoldThreshold = 0.1f;
    private double _holdTime = 0.0;

    public override void ProcessBPress(double delta)
    {
        if (!Input.IsActionPressed("ui_cancel"))
        {
            StopAnimation();
            Machine.TransitionToState("BikeStopWheelieRide");
        }
    }

    private void StopAnimation()
    {
        AnimationPlayer.Stop();
        AnimationPlayer.Play("RESET");
        Shadow.Visible = false;
        Dust.Play("default");
        Dust.Visible = false;
    }


    public override void EnterState()
    {
        base.EnterState();
        Shadow.Visible = true;
        Dust.Visible = true;
        AnimationPlayer.AnimationFinished += PlayDustAnimation;
        Dust.AnimationFinished += DustFinished;
        if (AnimationPlayer.IsPlaying()) return;
        AnimationPlayer.Play("wheelie_bounce");
    }


    public override void ExitState()
    {
        base.ExitState();
        AnimationPlayer.AnimationFinished -= PlayDustAnimation;
        Dust.AnimationFinished -= DustFinished;
    }

    public override bool IsMoving()
    {
        return true;
    }

    public override double GetMovementSpeed()
    {
        return Globals.Instance.AcroCyclingWheelieSpeed;
    }

    public override void StartIdling()
    {
        
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        return true;
    }

    protected override void ProcessPress(double delta)
    {
        if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
            !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
        {
            Machine.TransitionToState("BikeWheelieBounceIdle");
        }

        if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
            !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right") &&
            !Input.IsActionPressed("ui_cancel"))
        {
            StopAnimation();
            Machine.TransitionToState("BikeStopWheelieIdle");
            return;
        }

        if (Input.IsActionJustPressed("ui_accept"))
        {
            StopAnimation();
            Machine.TransitionToState("Idle");
        }

        if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
            Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
        {
            SetTargetPosition();
        }
    }
    
    private void PlayDustAnimation(StringName _)
    {
        Dust.Visible = true;
        Dust.Play("jump_dust");
        AnimationPlayer.Play("wheelie_bounce");
    }
    
    private void DustFinished()
    {
        Dust.Visible = false;
    }
}