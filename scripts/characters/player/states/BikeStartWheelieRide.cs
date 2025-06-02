using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStartWheelieRide : BikeWheelieTransitionState
{
	public override void ProcessBPress(double delta)
	{
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

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		return false;
	}

	protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			Machine.TransitionToState("BikeWheelieIdle");
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

}