using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStartWheelieRide : BikeWheelieTransitionState
{
	
	public override void _Process(double delta)
	{
		SetDirection();

		if (CheckForEnd(delta))
		{
			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelieRide");
			}
			else
			{
				Machine.TransitionToState("BikeWheelieRide");
				Machine.GetCurrentState<BikeWheelieRide>().SetUp(this);
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

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		return false;
	}

	private void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeWheelieIdle");
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

}