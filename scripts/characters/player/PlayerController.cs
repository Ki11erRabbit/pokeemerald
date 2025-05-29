using Godot;
using System;

namespace PokeEmerald.Characters.Player;

public partial class PlayerController : CharacterController
{
	[ExportCategory("Player Input")] 
	[Export]
	public double HoldThreshold = 0.1f;

	[Export] public double HoldTime = 0.0f;
	private bool _sameDirection = false;

	public void SetDirection()
	{
		
		if (Input.IsActionJustPressed("ui_up"))
		{
			_sameDirection = Direction.IsEqualApprox(Vector2.Up);
			Direction = Vector2.Up;
			TargetPosition = new Vector2(0, -16);
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			_sameDirection = Direction.IsEqualApprox(Vector2.Down);
			Direction = Vector2.Down;
			TargetPosition = new Vector2(0, 16);
		}
		else if (Input.IsActionJustPressed("ui_left"))
		{
			_sameDirection = Direction.IsEqualApprox(Vector2.Left);
			Direction = Vector2.Left;
			TargetPosition = new Vector2(-16, 0);
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			_sameDirection = Direction.IsEqualApprox(Vector2.Right);
			Direction = Vector2.Right;
			TargetPosition = new Vector2(16, 0);
		}
	}

	public void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (_sameDirection)
			{
				EmitSignal(SignalName.Walk);
			}
			else
			{
				EmitSignal(SignalName.Turn);
			}
			HoldTime = 0.0f;
		}

		if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
		    Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			HoldTime += delta;

			if (HoldTime > HoldThreshold)
			{
				EmitSignal(SignalName.Walk);
			}
		}
	}
}
