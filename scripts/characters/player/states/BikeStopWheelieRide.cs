using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStopWheelieRide : BikeWheelieTransitionState
{

	public override void ProcessBPress(double delta)
	{
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
		}
	}
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.WalkingSpeed;
	}
	
	public override bool IsMoving()
	{
		return true;
	}

	public override void StartIdling()
	{
		
	}



	protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			Machine.TransitionToState("BikeIdle");
		}
		
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			SetTargetPosition();
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