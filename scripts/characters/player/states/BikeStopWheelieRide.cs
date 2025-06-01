using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStopWheelieRide : BikeWheelieTransitionState
{
	public override void _Process(double delta)
	{
		SetDirection();

		if (CheckForEnd(delta))
		{
			if (Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeWheelieRide");
				Machine.GetCurrentState<BikeWheelieRide>().SetUp(this);
			}
			else
			{
				Debug.Log("Going back to bike ride");
				Machine.TransitionToState("BikeRide");
				Machine.GetCurrentState<BikeRide>().SetUp(this);
			}
			
			return;
		}
		
		ProcessPress(delta);
	}
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.AcroCyclingWheelieSpeed;
	}
	
	public override bool IsMoving()
	{
		return true;
	}

	public override void StartIdling()
	{
		
	}



	private void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeIdle");
			}
		}
		
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				SetTargetPosition();
			}
		}
	}
	
	protected override void SetEndFrame()
	{
		SetAnimationState([
			StateMachine.AnimationState.bike_acro_wheelie_start_up, 
			StateMachine.AnimationState.bike_acro_wheelie_start_left, 
			StateMachine.AnimationState.bike_acro_wheelie_start_right, 
			StateMachine.AnimationState.bike_acro_wheelie_start_down
		]);
	}
	protected override void SetStartFrame()
	{
		SetAnimationState([
			StateMachine.AnimationState.bike_acro_wheelie_start_up, 
			StateMachine.AnimationState.bike_acro_wheelie_start_left, 
			StateMachine.AnimationState.bike_acro_wheelie_start_right, 
			StateMachine.AnimationState.bike_acro_wheelie_start_down
		]);
	}

}