using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Run : CharacterState
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
		return Globals.Instance.RunningSpeed;
	}

	public override bool IsMoving()
	{
		return true;
	}

	public override void StartIdling()
	{
		Machine.TransitionToState("Idle");
	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.run_up, 
			StateMachine.AnimationState.run_left, 
			StateMachine.AnimationState.run_right, 
			StateMachine.AnimationState.run_down
		]);
		return false;
	}

	private void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("Idle");
			}
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("BikeIdle");
		}
		
		if ((Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right")) && !Colliding)
		{
			
			if (!Input.IsActionPressed("ui_cancel"))
			{
				if (!Colliding)
				{
					Machine.TransitionToState("Walk");
					var walk = Machine.GetCurrentState<Walk>();
					walk.SetUp(this);
				}
				else
				{
					Machine.TransitionToState("Idle");
					Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
				}
			}
			else
			{
				CheckForPositionAndCollison(new HeldDown());
			}
		}
	}
	
	private class Tapped : IReachedTargetPosition
	{
		public void Colliding(CharacterState self)
		{
			self.ResetTargetPosition();
			self.Machine.TransitionToState("Idle");
		}

		public void NotColliding(CharacterState self)
		{
			self.Machine.TransitionToState("Idle");
		}
	}
	
	private class HeldDown : IReachedTargetPosition
	{
		public void Colliding(CharacterState self)
		{
			self.EnterState();
		}

		public void NotColliding(CharacterState self)
		{
			self.Machine.TransitionToState("Idle");
			self.Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
		}
	}
}