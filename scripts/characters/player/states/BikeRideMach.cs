using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeRideMach : CharacterState
{
    public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
	}
    
	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.MachCyclingSpeed;
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
		if (GameState.GameState.RidingAcroBike())
		{
			SetAnimationState([
				StateMachine.AnimationState.bike_acro_ride_up, 
				StateMachine.AnimationState.bike_acro_ride_left, 
				StateMachine.AnimationState.bike_acro_ride_right, 
				StateMachine.AnimationState.bike_acro_ride_down
			]);
		}
		else
		{
			SetAnimationState([
				StateMachine.AnimationState.bike_mach_ride_up, 
				StateMachine.AnimationState.bike_mach_ride_left, 
				StateMachine.AnimationState.bike_mach_ride_right, 
				StateMachine.AnimationState.bike_mach_ride_down
			]);
		}
		
		return false;
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
				EnterState();
			}
		}
	}
}