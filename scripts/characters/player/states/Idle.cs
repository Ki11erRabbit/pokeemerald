using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Idle : PlayerIdleState
{
	public override void EnterState()
	{
		base.EnterState();
		GameState.GameState.StopRidingBike();

	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.idle_up, 
			StateMachine.AnimationState.idle_left, 
			StateMachine.AnimationState.idle_right, 
			StateMachine.AnimationState.idle_down
		]);
		return false;
	}
	

	protected override void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (SameDirection)
			{
				Machine.TransitionToState("Walk");
				if (!Colliding)
				{
					Machine.GetCurrentState<CharacterState>().SetUp(true);
				}
				else
				{
					Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
				}
			}
			else
			{
				Machine.TransitionToState("Turn");
				Machine.GetCurrentState<Turn>().SetUp(this);
			}
			HoldTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("BikeIdle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			HoldTime += delta;

			if (HoldTime > HoldThreshold)
			{
				if (SameDirection)
				{
					if (!Colliding)
					{
						Machine.TransitionToState("Walk");
					}
					else
					{
						Machine.TransitionToState("Walk");
						Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
					}
				}
				else if (Input.IsActionPressed("ui_cancel"))
				{
					if (!Colliding)
					{
						Machine.TransitionToState("Run");
					}
					else
					{
						Machine.TransitionToState("Walk");
						Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
					}
				}
				else
				{
					if (!Colliding)
					{
						Machine.TransitionToState("Walk");
					}
					else
					{
						Machine.TransitionToState("Walk");
						Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
					}
				}
			}
		}
	}
}