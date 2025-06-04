using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieRide : PlayerState
{
	private bool _sameDirection = false;
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
		
		LedgeRayCast.EnableCollision();
		RayCast.EnableCollision();
		_ledgeColliding = false;
		Colliding = false;
		CheckCollision();
	}

	public override void ExitState()
	{
		base.ExitState();
		LedgeRayCast.DisableCollision();
		RayCast.DisableCollision();
	}
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.RunningSpeed;
	}

    public override void SetUp(CharacterState state)
    {
	    TargetPosition = state.TargetPosition;
    }
    
    public override bool IsMoving()
    {
        return true;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("BikeWheelieIdle");
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_ride_up, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_left, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_right, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_down
        ]);
        return false;
    }
    

    protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeWheelieIdle");
			}
		} 
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		     !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right") &&
		     !Input.IsActionPressed("ui_cancel"))
		{
			Machine.TransitionToState("BikeStopWheelieIdle");
			return;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelieRide");
			}
			else if (AtTargetPosition())
			{
				EnterState();
			}
		}
	}

	protected override void CheckCollision()
	{
		base.CheckCollision();
		LedgeRayCast.TargetPosition = Controller.TargetPosition / 2;
		LedgeRayCast.CheckCollision();
	}
}