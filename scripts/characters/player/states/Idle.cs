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
    
	
    
	public override void Enter()
	{
		GameState.GameState.StopRidingBike();
	}

	public override void ExitState()
	{
		_holdTime = 0;
		_sameDirection = false;
	}

	public override double GetMovementSpeed()
	{
		return 0;
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
	
	protected override void SetDirection()
	{
		var lastDirection = Controller.Direction;
		if (Input.IsActionPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					Debug.Log($"Vectors are the same: {lastDirection} {Controller.Direction}");
					_sameDirection = true;
				}
				else
				{
					Debug.Log($"Vectors are not the same: {lastDirection} {Controller.Direction}");
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			if (Input.IsActionJustPressed("ui_down"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					Debug.Log($"Vectors are the same: {lastDirection} {Controller.Direction}");
					_sameDirection = true;
				}
				else
				{
					Debug.Log($"Vectors are not the same: {lastDirection} {Controller.Direction}");
					_sameDirection = false;
				}
			}
		} 
		if (Input.IsActionPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
			if (Input.IsActionJustPressed("ui_left"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					Debug.Log($"Vectors are the same: {lastDirection} {Controller.Direction}");
					_sameDirection = true;
				}
				else
				{
					Debug.Log($"Vectors are not the same: {lastDirection} {Controller.Direction}");
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
			if (Input.IsActionJustPressed("ui_right"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					Debug.Log($"Vectors are the same: {lastDirection} {Controller.Direction}");
					_sameDirection = true;
				}
				else
				{
					Debug.Log($"Vectors are not the same: {lastDirection} {Controller.Direction}");
					_sameDirection = false;
				}
			}
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
				if (_sameDirection)
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