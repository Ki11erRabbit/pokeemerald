using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public abstract partial class BikeWheelieTransitionState : CharacterState
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public double FrameThreshold = 0.3;
    protected double FrameTime = 0.0;
    [Export] public CharacterCollisonRayCast LedgeRayCast;
    private bool _ledgeColliding = false;

    public override void _Process(double delta)
    {
        if (_ledgeColliding && !Colliding)
        {
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

    public override void EnterState()
    {
        base.EnterState();
        SetStartFrame();
        FrameTime = 0.0;
        AnimatedSprite.Play(AnimationState.ToString());
		
        LedgeRayCast.EnableCollision();
        RayCast.EnableCollision();
        _ledgeColliding = false;
        Colliding = false;
        CheckCollision();
    }

    protected bool CheckForEnd(double delta)
    {
        FrameTime += delta;
        if (FrameTime >= FrameThreshold)
        {
            SetEndFrame();
            Debug.Log("Will be animating: " + AnimationState.ToString());
            
            Animation.PlayAnimation();
            return true;
        }
        Debug.Log("Will be animating: " + AnimationState.ToString());
        SetStartFrame();
        
        Animation.PlayAnimation();
        
        return false;
    }

    public override void ExitState()
    {
        base.ExitState();
        FrameTime = 0.0;
        LedgeRayCast.DisableCollision();
        RayCast.DisableCollision();
    }

    public override void SetUp(CharacterState state)
    {
        TargetPosition = state.TargetPosition;
    }
    
    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        if (FrameTime >= FrameThreshold)
        {
            SetEndFrame();
        }
        else
        {
            SetStartFrame();
        }
        return false;
    }

    protected virtual void SetStartFrame()
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_start_up, 
            StateMachine.AnimationState.bike_acro_wheelie_start_left, 
            StateMachine.AnimationState.bike_acro_wheelie_start_right, 
            StateMachine.AnimationState.bike_acro_wheelie_start_down
        ]);
    }
    protected virtual void SetEndFrame()
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_end_up, 
            StateMachine.AnimationState.bike_acro_wheelie_end_left, 
            StateMachine.AnimationState.bike_acro_wheelie_end_right, 
            StateMachine.AnimationState.bike_acro_wheelie_end_down
        ]);
    }
    
    protected override void SetDirection()
    {
        if (Input.IsActionPressed("ui_up"))
        {
            Controller.Direction = Vector2.Up;
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Controller.Direction = Vector2.Down;
            Controller.TargetPosition = new Vector2(0, 16);
        }
        if (Input.IsActionPressed("ui_left"))
        {
            Controller.Direction = Vector2.Left;
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            Controller.Direction = Vector2.Right;
            Controller.TargetPosition = new Vector2(16, 0);
        }
    }

    protected override void CheckCollision()
    {
        base.CheckCollision();
        LedgeRayCast.TargetPosition = Controller.TargetPosition;
        LedgeRayCast.CheckCollision();
    }
}