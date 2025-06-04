using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieBounceIdle : PlayerIdleState
{
    [ExportCategory("Nodes")]
    [Export] public AnimationPlayer AnimationPlayer;
    [Export] public Sprite2D Shadow;
    [Export] public AnimatedSprite2D Dust;
    [Export] public CharacterCollisonRayCast BunnyHopRayCast;
    private bool _bunnyColliding = false;

    public override void _Ready()
    {
        base._Ready();
        
        BunnyHopRayCast.Collision += SetBunnyColliding;
    }
    
    public void SetBunnyColliding(bool colliding)
    {
        _bunnyColliding = colliding;
    }

    public override void ProcessBPress(double delta)
    {
        if (!Input.IsActionPressed("ui_cancel") && !_bunnyColliding)
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
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_idle_up, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_left, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_right, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_down
        ]);
        return false;
    }
    
    protected override void ProcessPress(double delta)
    {
        if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
            Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
        {
            if (SameDirection)
            {
                Machine.TransitionToState("BikeWheelieBounceRide");
            }
            else
            {
                Machine.TransitionToState("BikeWheelieBounceTurn");
                Machine.GetCurrentState<CharacterState>().SetUp(this);
            }
            HoldTime = 0.0f;
        }

        if (Input.IsActionJustPressed("ui_accept") && !_bunnyColliding)
        {
            StopAnimation();
            Machine.TransitionToState("Idle");
        }
		
        if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
            Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
        {
            HoldTime += delta;

            if (HoldTime > HoldThreshold)
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