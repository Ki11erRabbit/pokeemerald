using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public abstract partial class PlayerIdleState : CharacterState
{
    [ExportCategory("Vars")] 
    [Export] public double HoldThreshold = 0.1f;
    protected bool SameDirection = false;
    protected double HoldTime = 0;
    
    public override void ExitState()
    {
        HoldTime = 0;
        SameDirection = false;
        Colliding = false;
    }
    
    public override double GetMovementSpeed()
    {
        return 0;
    }

    public override void StartIdling()
    {
		
    }
    
    public override bool IsMoving()
    {
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
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
					SameDirection = true;
				}
				else
				{
					SameDirection = false;
				}
			}
		}
	}
}