using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Run : CharacterState
{
	[Export] public CharacterCollisonRayCast LedgeRayCast;
	private bool _ledgeColliding = false;

	public override void _Process(double delta)
	{
		if (_ledgeColliding && !Colliding)
		{
			Machine.TransitionToState("LedgeJump");
			ResetTargetPosition();
			Machine.GetCurrentState<CharacterState>().SetUp(this);
			_ledgeColliding = false;
			return;
		}
		base._Process(delta);
	}

	public override void EnterState()
	{
		base.EnterState();
		
		LedgeRayCast.EnableCollision();
		RayCast.EnableCollision();
		_ledgeColliding = false;
		Colliding = false;
		CheckCollision();
	}

	public override void ExitState()
	{
		base.ExitState();
		LedgeRayCast.DisableCollision();
		RayCast.DisableCollision();
	}

	public override void _Ready()
	{
		base._Ready();
		LedgeRayCast.Collision += SetLedgeColliding;
	}
	
	public virtual void SetLedgeColliding(bool colliding, GodotObject what)
	{
		_ledgeColliding = colliding;
	}
	
	public override void ProcessBPress(double delta)
	{
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		     Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			
			if (!Input.IsActionPressed("ui_cancel"))
			{
				if (!Colliding)
				{
					Machine.TransitionToState("Walk");
					var walk = Machine.GetCurrentState<Walk>();
					walk.SetUp(this);
				}
				else
				{
					Machine.TransitionToState("Idle");
					Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
				}
			}
		}
	}

	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}
	public override double GetMovementSpeed()
	{
		return Globals.Instance.RunningSpeed;
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

	protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			Machine.TransitionToState("Idle");
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("BikeIdle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{

			if (!Colliding)
			{
				EnterState();
			}
			else
			{
				Machine.TransitionToState("Idle");
				Machine.GetCurrentState<CharacterState>().ResetTargetPosition();
			}
		}
	}

	protected override void CheckCollision()
	{
		base.CheckCollision();
		LedgeRayCast.TargetPosition = Controller.TargetPosition;
		LedgeRayCast.CheckCollision();
	}
}