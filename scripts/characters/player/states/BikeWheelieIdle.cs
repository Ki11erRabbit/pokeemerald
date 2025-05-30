using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieIdle : CharacterState
{
    [Export] public double HoldThreshold = 0.1;
	private bool _sameDirection = false;
	private double _holdTime = 0.0;
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
			_holdTime = 0.0f;
		}

		if (Input.IsActionJustPressed("ui_accept"))
		{
			Machine.TransitionToState("Idle");
		}
		
		if (!Input.IsActionPressed("ui_cancel"))
		{
			Machine.TransitionToState("BikeStopWheelieIdle");
			return;
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