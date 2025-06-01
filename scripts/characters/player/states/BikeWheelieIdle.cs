using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieIdle : CharacterState
{
    [Export] public double HoldThreshold = 0.1;
    [Export] public double BounceHoldThreshold = 1;
	private bool _sameDirection = false;
	private double _holdTime = 0.0;
	private double _bounceHoldTime = 0.0;
	public override double GetMovementSpeed()
	{
		return 0;
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
            StateMachine.AnimationState.bike_acro_wheelie_idle_up, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_left, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_right, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_down
        ]);
        return false;
    }
    


	private void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (_sameDirection)
			{
				Machine.TransitionToState("BikeWheelieRide");
			}
			else
			{
				Machine.TransitionToState("BikeWheelieTurn");
				Machine.GetCurrentState<CharacterState>().SetUp(this);
			}
			_holdTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (!Input.IsActionPressed("ui_cancel"))
		{
			Machine.TransitionToState("BikeStopWheelieIdle");
			_bounceHoldTime = 0.0;
			return;
		}
		if (Input.IsActionPressed("ui_cancel"))
		{
			_bounceHoldTime += delta;
			Debug.Log("Increasing bounce hold time");
			if (_bounceHoldTime > BounceHoldThreshold)
			{
				Machine.TransitionToState("BikeWheelieBounceIdle");
				_bounceHoldTime = 0.0;
				return;
			}
		}
	
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_holdTime += delta;

			if (_holdTime > HoldThreshold)
			{
				Machine.TransitionToState("BikeWheelieRide");
			}
		}
	}

    public override void _Process(double delta)
    {
        SetDirection();
        ProcessPress(delta);
    }
}