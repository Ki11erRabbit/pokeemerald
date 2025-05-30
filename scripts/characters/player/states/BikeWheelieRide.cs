using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieRide : CharacterState
{
	private bool _sameDirection = false;
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
			Debug.Log("Pressing down");
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
		
		if (!Input.IsActionPressed("ui_cancel"))
		{
			Machine.TransitionToState("BikeStopWheelie");
			return;
		}
		
		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{

			if (!Input.IsActionPressed("ui_cancel"))
			{
				Machine.TransitionToState("BikeStopWheelie");
			}
			else if (AtTargetPosition())
			{
				EnterState();
			}
		}
	}

    public override void _Process(double delta)
    {
        SetDirection();
        ProcessPress(delta);
    }
}