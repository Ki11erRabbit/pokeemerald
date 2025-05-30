using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStopWheelie : CharacterState
{
	[Export] public AnimatedSprite2D AnimatedSprite;
	private bool _isMoving = false;
	private bool _shouldProcess = false;
    public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
		_shouldProcess = true;
	}
    
	public override void Enter()
	{
		_isMoving = false;
	}

	public override void CustomReady()
	{
		AnimatedSprite.AnimationFinished += AnimationFinished;
	}

	public override void Move(double delta)
	{
		delta *= Globals.Instance.TileSize * Globals.Instance.AcroCyclingWheelieSpeed;
		_character.Position = _character.Position.MoveToward(TargetPosition, (float)delta);
	}
	
	public override bool IsMoving()
	{
		return _isMoving;
	}

	public override void StartIdling()
	{
		
	}

	public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
	{
		SetAnimationState([
			StateMachine.AnimationState.bike_acro_wheelie_start_up, 
			StateMachine.AnimationState.bike_acro_wheelie_start_left, 
			StateMachine.AnimationState.bike_acro_wheelie_start_right, 
			StateMachine.AnimationState.bike_acro_wheelie_start_down
		]);
		animatedSprite.PlayBackwards(AnimationState.ToString());
		return true;
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
				_isMoving = false;
			}
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			_shouldProcess = false;
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{

			_isMoving = true;
			
			if (AtTargetPosition())
			{
			}
		}
	}

	public void AnimationFinished()
	{
		if (CanProcess() && _shouldProcess)
		{
			Debug.Log("Stop Wheelie is proccessing");
			if (_isMoving)
			{
				_shouldProcess = false;
				Machine.TransitionToState("BikeRide");
			}
			else
			{
				_shouldProcess = false;
				Machine.TransitionToState("BikeIdle");
			}
		}
	}
}