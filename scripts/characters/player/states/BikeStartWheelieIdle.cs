using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStartWheelieIdle : BikeWheelieTransitionState
{
	
	public override void _Process(double delta)
	{
		SetDirection();

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
			
			return;
		}
		
		ProcessPress(delta);
	}
	public override void Move(double delta)
	{
		
	}
	
	public override bool IsMoving()
	{
		return false;
	}

	public override void StartIdling()
	{
	}


	private void ProcessPress(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
	}
}