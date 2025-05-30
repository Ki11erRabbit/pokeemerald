using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Run : CharacterState
{
    [ExportCategory("Controller")] 
	[Export]
	public PlayerController Controller;

	[ExportCategory("Vars")] 
	[Export] public double HoldThreshold = 0.1f;
	private bool _sameDirection = false;
	private double _holdTime = 0;
	
	public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
	}
	
	public override void ExitState()
	{
		_holdTime = 0;
		_sameDirection = false;
	}
	
	public override void Move(double delta)
	{
		delta *= Globals.Instance.TileSize * Globals.Instance.RunningSpeed;
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
			StateMachine.AnimationState.run_up, 
			StateMachine.AnimationState.run_left, 
			StateMachine.AnimationState.run_right, 
			StateMachine.AnimationState.run_down
		]);
		return false;
	}

	private void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			
			_holdTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("BikeIdle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_holdTime += delta;

			if (_holdTime > HoldThreshold)
			{
				if (!Input.IsActionPressed("ui_cancel"))
				{
					Machine.TransitionToState("Walk");
				}
				else
				{
					EnterState();
				}
			}
		}
	}
}