using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeIdle : CharacterState
{
	[Export] public double HoldThreshold = 0.1;
	private bool _sameDirection = false;
	private double _holdTime = 0.0;
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
    
    protected override void SetDirection()
    {
	    _lastDirection = Controller.Direction;
		if (Input.IsActionPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			if (_lastDirection.IsEqualApprox(Vector2.Right) || _lastDirection.IsEqualApprox(Vector2.Left))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			if (Input.IsActionJustPressed("ui_down"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Left) || _lastDirection.IsEqualApprox(Vector2.Right))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		} 
		if (Input.IsActionPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
			if (Input.IsActionJustPressed("ui_left"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Up) || _lastDirection.IsEqualApprox(Vector2.Down))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
		else if (Input.IsActionPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
			if (Input.IsActionJustPressed("ui_right"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Up) || _lastDirection.IsEqualApprox(Vector2.Down))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
		
		if (Input.IsActionJustPressed("ui_up"))
		{
			Controller.Direction = Vector2.Up;
			Controller.TargetPosition = new Vector2(0, -16);
			if (Input.IsActionJustPressed("ui_up"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Left) || _lastDirection.IsEqualApprox(Vector2.Right))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			Controller.Direction = Vector2.Down;
			Controller.TargetPosition = new Vector2(0, 16);
			if (Input.IsActionJustPressed("ui_down"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Left) || _lastDirection.IsEqualApprox(Vector2.Right))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		} 
		if (Input.IsActionJustPressed("ui_left"))
		{
			Controller.Direction = Vector2.Left;
			Controller.TargetPosition = new Vector2(-16, 0);
			if (Input.IsActionJustPressed("ui_left"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Up) || _lastDirection.IsEqualApprox(Vector2.Down))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			Controller.Direction = Vector2.Right;
			Controller.TargetPosition = new Vector2(16, 0);
			if (Input.IsActionJustPressed("ui_right"))
			{
				if (_lastDirection.IsEqualApprox(Controller.Direction))
				{
					_sameDirection = true;
				}
				else
				{
					_sameDirection = false;
				}
			}
			
			if (_lastDirection.IsEqualApprox(Vector2.Up) || _lastDirection.IsEqualApprox(Vector2.Down))
			{
				if (Input.IsActionJustPressed("ui_cancel"))
				{
					_facingDirection = _lastDirection;
					_shouldJump = true;
				}
				else
				{
					_facingDirection = Vector2.Zero;
				}
			}
		}
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
			if (_sameDirection)
			{
				Machine.TransitionToState("BikeRide");
			}
			else
			{
				Machine.TransitionToState("BikeTurn");
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