using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieRide : CharacterState
{
	private bool _sameDirection = false;
	
	public override double GetMovementSpeed()
	{
		return Globals.Instance.AcroCyclingWheelieSpeed;
	}

    public override void SetUp(CharacterState state)
    {
	    TargetPosition = state.TargetPosition;
    }
    
    public override bool IsMoving()
    {
        return true;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("BikeWheelieIdle");
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_ride_up, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_left, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_right, 
            StateMachine.AnimationState.bike_acro_wheelie_ride_down
        ]);
        return false;
    }
    

    protected override void ProcessPress(double delta)
	{
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		    !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
		{
			if (AtTargetPosition())
			{
				Machine.TransitionToState("BikeWheelieIdle");
			}
		} 
		if (!Input.IsActionPressed("ui_up") && !Input.IsActionPressed("ui_down") &&
		     !Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right") &&
		     !Input.IsActionPressed("ui_cancel"))
		{
			Machine.TransitionToState("BikeStopWheelieIdle");
			return;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelieRide");
			}
			else if (AtTargetPosition())
			{
				EnterState();
			}
		}
	}

}