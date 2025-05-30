using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeRide : CharacterState
{
	[Export] public double SpeedUpThreshold = 0.3;
	private double _speedUpTime = 0.0;
    public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
	}
	
	public override void Move(double delta)
	{
		delta *= Globals.Instance.TileSize * Globals.Instance.AcroCyclingSpeed;
		_character.Position = _character.Position.MoveToward(TargetPosition, (float)delta);
	}
	
	public override bool IsMoving()
	{
		return true;
	}

	public override void StartIdling()
	{
		Machine.TransitionToState("BikeIdle");
	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.bike_acro_ride_up, 
			StateMachine.AnimationState.bike_acro_ride_left, 
			StateMachine.AnimationState.bike_acro_ride_right, 
			StateMachine.AnimationState.bike_acro_ride_down
		]);
		
		return false;
	}

	private void SetDirection()
	{
		if (Input.IsActionJustPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
		}
		else if (Input.IsActionJustPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
		}
	}

	private void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeIdle");
			}
			_speedUpTime = 0;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_speedUpTime += delta;
			
			if (Input.IsActionPressed("ui_cancel"))
			{
				if (GameState.GameState.RidingAcroBike()) Machine.TransitionToState("BikeStartWheelie");
			}

			if (_speedUpTime > SpeedUpThreshold && !GameState.GameState.RidingAcroBike())
			{
				Machine.TransitionToState("BikeRideMach");
			}
			else if (AtTargetPosition())
			{
				EnterState();
			}
		}
	}
}