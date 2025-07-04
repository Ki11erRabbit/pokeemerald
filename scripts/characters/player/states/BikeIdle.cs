using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeIdle : PlayerIdleState
{
	private Vector2 _facingDirection;
	private bool _shouldJump = false;
	private Vector2 _lastDirection;

	public override void _Process(double delta)
	{
		if (AtTargetPosition() || AtStartPosition())
		{
			SetDirection();
			CheckCollision();
			ProcessPress(delta);
		}
		ProcessBPress(delta);
	}

	public override void SetUp(Vector2 direction)
	{
		Controller.Direction = direction;
		_facingDirection = Vector2.Zero;
	}

	public override void EnterState()
	{
		base.EnterState();
		GameState.GameState.RideBike();
	}

	public override void ExitState()
	{
		base.ExitState();
		_facingDirection = Vector2.Zero;
	}

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {

        if (_facingDirection == Vector2.Zero)
        {
	        SetAnimationState([
		        StateMachine.AnimationState.bike_idle_up, 
		        StateMachine.AnimationState.bike_idle_left, 
		        StateMachine.AnimationState.bike_idle_right, 
		        StateMachine.AnimationState.bike_idle_down
	        ]);
        }
        else
        {
	        if (_facingDirection == Vector2.Up)
	        {
		        if (Controller.Direction == Vector2.Left)
		        {
			        AnimationState = AnimationState.bike_idle_up;
		        }
		        else if (Controller.Direction == Vector2.Right)
		        {
			        AnimationState = AnimationState.bike_idle_up;
		        }
			    
	        }
	        else if (_facingDirection == Vector2.Left)
	        {
		        if (Controller.Direction == Vector2.Up)
		        {
			        AnimationState = AnimationState.bike_idle_left;
		        }
		        else if (Controller.Direction == Vector2.Down)
		        {
			        AnimationState = AnimationState.bike_idle_left;
		        }
	        }
	        else if (_facingDirection == Vector2.Right)
	        {
		        if (Controller.Direction == Vector2.Up)
		        {
			        AnimationState = AnimationState.bike_idle_right;
		        }
		        else if (Controller.Direction == Vector2.Down)
		        {
			        AnimationState = AnimationState.bike_idle_right;
		        }
	        }
	        else if (_facingDirection == Vector2.Down)
	        {
		        if (Controller.Direction == Vector2.Left)
		        {
			        AnimationState = AnimationState.bike_idle_down;
		        }
		        else if (Controller.Direction == Vector2.Right)
		        {
			        AnimationState = AnimationState.bike_idle_down;
		        }
	        }
        }
        
        return false;
    }
    
    

    protected override void ProcessPress(double delta)
	{
		if (_shouldJump && GameState.GameState.RidingAcroBike())
		{
			_shouldJump = false;

			Machine.GetState<CharacterState>("BikeSideHop").SetUp(_facingDirection);
			Machine.TransitionToState("BikeSideHop");
			return;
		}
		
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (SameDirection)
			{
				Machine.TransitionToState("BikeRide");
			}
			else
			{
				Machine.TransitionToState("BikeTurn");
				Machine.GetCurrentState<CharacterState>().SetUp(this);
			}
			HoldTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			HoldTime += delta;

			if (HoldTime > HoldThreshold)
			{
				_lastDirection = Controller.Direction;
				if (Input.IsActionPressed("ui_cancel"))
				{
					if (GameState.GameState.RidingAcroBike() && !_shouldJump)
					{
						Machine.TransitionToState("BikeStartWheelieRide");
						Machine.GetCurrentState<BikeStartWheelieRide>().SetUp(this);
						return;
					}
				}
				
				Machine.TransitionToState("BikeRide");
				
			}
		}
		else if (Input.IsActionPressed("ui_cancel"))
		{
			if (GameState.GameState.RidingAcroBike())
			{
				Machine.TransitionToState("BikeStartWheelieIdle");
			}
		}
	}
}