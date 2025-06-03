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

    [Export] public CharacterCollisonRayCast LedgeRayCast;
    private bool _ledgeColliding = false;

    public override void _Process(double delta)
    {
        if (_ledgeColliding && !Colliding)
        {
            AnimationPlayer.Stop();
            AnimationPlayer.Play("RESET");
            Dust.Visible = false;
            Machine.TransitionToState("LedgeJump");
            ResetTargetPosition();
            Machine.GetCurrentState<CharacterState>().SetUp(this);
            _ledgeColliding = false;
            return;
        }
        base._Process(delta);
    }

    public override void _Ready()
    {
        base._Ready();
        LedgeRayCast.Collision += SetLedgeColliding;
    }
	
    public virtual void SetLedgeColliding(bool colliding, GodotObject what)
    {
        _ledgeColliding = colliding;
    }
    
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
		
        LedgeRayCast.EnableCollision();
        RayCast.EnableCollision();
        _ledgeColliding = false;
        Colliding = false;
        CheckCollision();
        
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
        LedgeRayCast.DisableCollision();
        RayCast.DisableCollision();
    }

    public override bool IsMoving()
    {
        return true;
    }

    public override double GetMovementSpeed()
    {
        return Globals.Instance.WalkingSpeed;
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
    
    protected override void CheckCollision()
    {
        base.CheckCollision();
        LedgeRayCast.TargetPosition = Controller.TargetPosition;
        LedgeRayCast.CheckCollision();
    }
}