using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStopWheelieIdle : BikeWheelieTransitionState
{

	
	public override void ProcessBPress(double delta)
	{
		if (CheckForEnd(delta))
		{
			Machine.TransitionToState("BikeIdle");
			
		}
	}

	public override double GetMovementSpeed()
	{
		return 0;
	}
	
	public override bool IsMoving()
	{
		return false;
	}

	public override void StartIdling()
	{
		
	}

	protected override void ProcessPress(double delta)
	{
		
		
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
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