using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeRide : PlayerState
{
	[Export] public double SpeedUpThreshold = 0.3;
	private double _speedUpTime = 0.0;
   
    
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

	public override void EnterState()
	{
		base.EnterState();
		
		LedgeRayCast.EnableCollision();
		RayCast.EnableCollision();
		_ledgeColliding = false;
		Colliding = false;
		CheckCollision();
	}
	
	public override void _Ready()
	{
		base._Ready();
		LedgeRayCast.Collision += SetLedgeColliding;
	}
	
	public virtual void SetLedgeColliding(bool colliding)
	{
		_ledgeColliding = colliding;
	}
	
	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}

	public override void ExitState()
	{
		base.ExitState();
		_speedUpTime = 0;
		LedgeRayCast.DisableCollision();
		RayCast.DisableCollision();
	}
	public override double GetMovementSpeed()
	{
		return Globals.Instance.AcroCyclingSpeed;
	}
	
	public override bool IsMoving()
	{
		return true;
	}

	public override void StartIdling()
	{
		Machine.TransitionToState("BikeIdle");
	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.bike_acro_ride_up, 
			StateMachine.AnimationState.bike_acro_ride_left, 
			StateMachine.AnimationState.bike_acro_ride_right, 
			StateMachine.AnimationState.bike_acro_ride_down
		]);
		
		return false;
	}



	protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			Machine.TransitionToState("BikeIdle");
			_speedUpTime = 0;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_speedUpTime += delta;
			
			if (Input.IsActionPressed("ui_cancel"))
			{
				if (GameState.GameState.RidingAcroBike())
				{
					Machine.TransitionToState("BikeStartWheelieRide");
					Machine.GetCurrentState<BikeStartWheelieRide>().SetUp(this);
				}
			}

			if (_speedUpTime > SpeedUpThreshold && !GameState.GameState.RidingAcroBike() && GameState.GameState.Instance.RidingBike)
			{
				Machine.TransitionToState("BikeRideMach");
				Machine.GetCurrentState<BikeRideMach>().SetUp(this);
				_speedUpTime = 0;
			}
			else
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