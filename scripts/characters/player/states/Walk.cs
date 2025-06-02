using Godot;
using System;
using PokeEmerald.Characters.StateMachine;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Walk : CharacterState
{
	private bool _tapped = false;

	public override void ProcessBPress(double delta)
	{
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			
			if (Input.IsActionPressed("ui_cancel"))
			{
				if (!Colliding)
				{
					Machine.TransitionToState("Run");
					Machine.GetCurrentState<Run>().SetUp(this);
				}
				else
				{
					Machine.TransitionToState("Idle");
					Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
				}
			}
		}
	}

	public override void SetUp(bool tapped)
	{
		_tapped = tapped;
	}
	
	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}

	public override void ExitState()
	{
		base.ExitState();
		_tapped = false;
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
		Machine.TransitionToState("Idle");
	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.walk_up, 
			StateMachine.AnimationState.walk_left, 
			StateMachine.AnimationState.walk_right, 
			StateMachine.AnimationState.walk_down
		]);
		return false;
	}
	
	protected override void ProcessPress(double delta)
	{
		if (_tapped)
		{
			Machine.TransitionToState("Idle");
			return;
		}
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
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		     Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			
			if (Input.IsActionPressed("ui_cancel"))
			{
				if (!Colliding)
				{
					Machine.TransitionToState("Run");
					Machine.GetCurrentState<Run>().SetUp(this);
				}
				else
				{
					Machine.TransitionToState("Idle");
					Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
				}

				return;
			}
			EnterState();
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
			self.ResetTargetPosition();
		}
	}
}
