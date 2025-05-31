using Godot;
using System;
using PokeEmerald.Characters.StateMachine;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Walk : CharacterState
{
	private bool _tapped = false;
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

	public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
	}
	
	public override void Move(double delta)
	{
		delta *= Globals.Instance.TileSize * Globals.Instance.WalkingSpeed;
		_character.Position = _character.Position.MoveToward(TargetPosition, (float)delta);
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


	private void ProcessPress(double delta)
	{
		if (_tapped)
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("Idle");
			}
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
				Machine.TransitionToState("Run");
				Machine.GetCurrentState<Walk>().SetUp(this);
			}
			
			if (AtTargetPosition())
			{
				EnterState();
			}
		}
	}
	
}
