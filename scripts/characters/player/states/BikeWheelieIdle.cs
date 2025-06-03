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

	public override void ProcessBPress(double delta)
	{
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
			}
		}
	}

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
    
    protected override void SetDirection()
    {
	    var lastDirection = Controller.Direction;
		if (Input.IsActionPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			if (Input.IsActionJustPressed("ui_down"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		} 
		if (Input.IsActionPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
			if (Input.IsActionJustPressed("ui_left"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
			if (Input.IsActionJustPressed("ui_right"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
		
		if (Input.IsActionJustPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			if (Input.IsActionJustPressed("ui_down"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		} 
		if (Input.IsActionJustPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
			if (Input.IsActionJustPressed("ui_left"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
			if (Input.IsActionJustPressed("ui_right"))
			{
				if (lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
		}
    }


    protected override void ProcessPress(double delta)
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
}