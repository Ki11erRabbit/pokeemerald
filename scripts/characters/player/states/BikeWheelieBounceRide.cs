using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieBounceRide : PlayerState
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
    [Export] public CharacterCollisonRayCast BunnyHopRayCast;
    private bool _bunnyColliding = false;

    public override void _Process(double delta)
    {
        if (_ledgeColliding && !Colliding && !_bunnyColliding)
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
        if (AtTargetPosition() || AtStartPosition())
        {
            SetDirection();
            CheckCollision();
            ProcessPress(delta);
        }
        ProcessBPress(delta);
    }

    public override void _Ready()
    {
        base._Ready();
        LedgeRayCast.Collision += SetLedgeColliding;
        BunnyHopRayCast.Collision += SetBunnyColliding;
    }
	
    public virtual void SetLedgeColliding(bool colliding)
    {
        _ledgeColliding = colliding;
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
            !Input.IsActionPressed("ui_cancel") && !_bunnyColliding)
        {
            StopAnimation();
            Machine.TransitionToState("BikeStopWheelieIdle");
            return;
        }

        if (Input.IsActionJustPressed("ui_accept") && !_bunnyColliding)
        {
            StopAnimation();
            Machine.TransitionToState("Idle");
        }

        if ((Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
            Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right")))
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
        LedgeRayCast.TargetPosition = Controller.TargetPosition / 2;
        LedgeRayCast.CheckCollision();
        BunnyHopRayCast.TargetPosition = Controller.TargetPosition / 2;
        BunnyHopRayCast.CheckCollision();
    }

    public override void Move(double delta)
    {
        CheckCollision();
        Debug.Log("BunnyColliding: " + _bunnyColliding);
        if (Colliding && AtStartPosition() && !_bunnyColliding)
        {
            ResetTargetPosition();
            return;
        }
        delta *= Globals.Instance.TileSize * GetMovementSpeed();
        Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);
        var pos = Character.Position;
        Character.Position = new Vector2(Mathf.Floor(pos.X), Mathf.Floor(pos.Y));
    }
}