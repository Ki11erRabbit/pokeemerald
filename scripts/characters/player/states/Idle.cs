using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Idle : CharacterState
{
	[ExportCategory("Vars")] 
	[Export] public double HoldThreshold = 0.1f;
	private bool _sameDirection = false;
	[Export] private double _holdTime = 0;
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
		
	}

	public override void StartIdling()
	{
		
	}

	public override bool IsMoving()
	{
		return false;
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

	private void SetDirection()
	{
		if (Input.IsActionJustPressed("ui_up"))
		{
			_sameDirection = Controller.Direction.IsEqualApprox(Vector2.Up);
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			_sameDirection = Controller.Direction.IsEqualApprox(Vector2.Down);
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			Debug.Log("Pressing down");
		}
		else if (Input.IsActionJustPressed("ui_left"))
		{
			_sameDirection = Controller.Direction.IsEqualApprox(Vector2.Left);
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			_sameDirection = Controller.Direction.IsEqualApprox(Vector2.Right);
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
		}
	}

	private void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (_sameDirection)
			{
				Machine.TransitionToState("Walk");
				Machine.GetCurrentState<CharacterState>().SetTargetPosition();
				Debug.Log("Walking via same direction");
			}
			else
			{
				Machine.TransitionToState("Turn");
				Machine.GetCurrentState<Turn>().SetUp(this);
				Debug.Log("Turning");
			}
			_holdTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
		
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_holdTime += delta;

			if (_holdTime > HoldThreshold)
			{
				if (Input.IsActionPressed("ui_cancel"))
				{
					Machine.TransitionToState("Run");
					Machine.GetCurrentState<CharacterState>().SetTargetPosition();
					Debug.Log("Running via holding down");
				}
				else
				{
					Machine.TransitionToState("Walk");
					Machine.GetCurrentState<CharacterState>().SetTargetPosition();
					Debug.Log("\tWalking via holding down");
				}
			}
		}
	}
}