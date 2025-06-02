using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieBounceIdle : CharacterState
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
            Machine.TransitionToState("BikeStopWheelieIdle");
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
        return false;
    }

    public override double GetMovementSpeed()
    {
        return 0;
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
        if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
            Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
        {
            Machine.TransitionToState("BikeWheelieBounceRide");
            _holdTime = 0.0f;
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
            _holdTime += delta;

            if (_holdTime > HoldThreshold)
            {
                Machine.TransitionToState("BikeWheelieBounceRide");
            }
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