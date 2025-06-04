using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeRideMach : PlayerState
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

	public override void _Ready()
	{
		base._Ready();
		LedgeRayCast.Collision += SetLedgeColliding;
	}
	
	public virtual void SetLedgeColliding(bool colliding)
	{
		_ledgeColliding = colliding;
	}
	public override void SetUp(CharacterState state)
	{
		TargetPosition = state.TargetPosition;
	}
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.MachCyclingSpeed;
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
		if (GameState.GameState.RidingAcroBike())
		{
			SetAnimationState([
				StateMachine.AnimationState.bike_acro_ride_up, 
				StateMachine.AnimationState.bike_acro_ride_left, 
				StateMachine.AnimationState.bike_acro_ride_right, 
				StateMachine.AnimationState.bike_acro_ride_down
			]);
		}
		else
		{
			SetAnimationState([
				StateMachine.AnimationState.bike_mach_ride_up, 
				StateMachine.AnimationState.bike_mach_ride_left, 
				StateMachine.AnimationState.bike_mach_ride_right, 
				StateMachine.AnimationState.bike_mach_ride_down
			]);
		}
		
		return false;
	}


	protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			Machine.TransitionToState("BikeIdle");
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{

			EnterState();
		}
	}
	
	protected override void CheckCollision()
	{
		base.CheckCollision();
		LedgeRayCast.TargetPosition = Controller.TargetPosition;
		LedgeRayCast.CheckCollision();
	}
}