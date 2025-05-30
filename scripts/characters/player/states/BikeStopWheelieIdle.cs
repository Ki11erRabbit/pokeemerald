using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeStopWheelieIdle : CharacterState
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
		_shouldProcess = true;
	}

	public override void ExitState()
	{
		base.ExitState();
		_shouldProcess = false;
	}

	public override void CustomReady()
	{
		AnimatedSprite.AnimationFinished += AnimationFinished;
	}

	public override void Move(double delta)
	{
		
	}
	
	public override bool IsMoving()
	{
		return false;
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


	private void ProcessPress(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			_shouldProcess = false;
			Machine.TransitionToState("Idle");
		}
	}

	public void AnimationFinished()
	{
		if (CanProcess() && _shouldProcess)
		{
			_shouldProcess = false;
			Machine.TransitionToState("BikeIdle");
		}
	}
}