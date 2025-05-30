using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeIdle : CharacterState
{
	[Export] public double HoldThreshold = 0.1;
	private bool _sameDirection = false;
	private double _holdTime = 0.0;

	public override void Enter()
	{
		GameState.GameState.RideBike();
		System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
		Debug.Log(trace.ToString());
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
            StateMachine.AnimationState.bike_idle_up, 
            StateMachine.AnimationState.bike_idle_left, 
            StateMachine.AnimationState.bike_idle_right, 
            StateMachine.AnimationState.bike_idle_down
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
				Machine.TransitionToState("BikeRide");
			}
			_holdTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		if (Input.IsActionPressed("ui_cancel"))
		{
			if (GameState.GameState.RidingAcroBike())
			{
				Machine.TransitionToState("BikeStartWheelie");
				Debug.Log("Starting wheelie");
				return;
			}
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_holdTime += delta;

			if (_holdTime > HoldThreshold)
			{
				
				Machine.TransitionToState("BikeRide");
				
			}
		}
	}

    public override void _Process(double delta)
    {
        SetDirection();
        ProcessPress(delta);
    }
}