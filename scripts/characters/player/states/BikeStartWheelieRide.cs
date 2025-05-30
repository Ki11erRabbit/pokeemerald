using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStartWheelieRide : CharacterState
{
	[Export] public AnimatedSprite2D AnimatedSprite;
	private bool _shouldProcess = false;

	
	public override void _Process(double delta)
	{
		SetDirection();
		ProcessPress(delta);
		_shouldProcess = true;
	}
	
	public override void Enter()
	{
		base.Enter();
		_shouldProcess = true;
	}

	public override void ExitState()
	{
		base.ExitState();
		_shouldProcess = false;
	}

	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
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
		return true;
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
		return false;
	}

	private void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeWheelieIdle");
			}
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				SetTargetPosition();
			}
		}
	}

	public void AnimationFinished()
	{
		if (CanProcess() && _shouldProcess)
		{
			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelieRide");
			}
			else
			{
				Machine.TransitionToState("BikeWheelieRide");
				Machine.GetCurrentState<BikeWheelieRide>().SetUp(this);
			}
		}
	}
}