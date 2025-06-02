using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStartWheelieIdle : BikeWheelieTransitionState
{


	public override void ProcessBPress(double delta)
	{
		if (CheckForEnd(delta))
		{
			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelieIdle");
			}
			else
			{
				Machine.TransitionToState("BikeWheelieIdle");
			}
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
}