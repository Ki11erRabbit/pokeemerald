using Godot;
using System;

namespace PokeEmerald.Characters.Player;

public partial class PlayerController : CharacterController
{
	[ExportCategory("Player Input")] 
	[Export]
	public double HoldThreshold = 0.1f;

	[Export] public double HoldTime = 0.0f;

	public void SetDirection()
	{
		if (Input.IsActionJustPressed("ui_up"))
		{
			Direction = Vector2.Up;
			TargetPosition = new Vector2(0, -16);
		}
		else if (Input.IsActionJustPressed("ui_down"))
		{
			Direction = Vector2.Down;
			TargetPosition = new Vector2(0, 16);
		}
		else if (Input.IsActionJustPressed("ui_left"))
		{
			Direction = Vector2.Left;
			TargetPosition = new Vector2(-16, 0);
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			Direction = Vector2.Right;
			TargetPosition = new Vector2(16, 0);
		}
	}

	public void ProcessPress(double delta)
	{
		if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
		    Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
		{
			if (HoldTime >= HoldThreshold)
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
