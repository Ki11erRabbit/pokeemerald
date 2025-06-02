using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeRide : CharacterState
{
	[Export] public double SpeedUpThreshold = 0.3;
	private double _speedUpTime = 0.0;
   
    
	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}

	public override void ExitState()
	{
		base.ExitState();
		_speedUpTime = 0;
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
				Machine.GetCurrentState<BikeStartWheelieRide>().SetUp(this);
				_speedUpTime = 0;
			}
			else
			{
				EnterState();
			}
		}
	}
}