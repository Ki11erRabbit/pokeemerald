using Godot;
using System;
using Game.Utilities;

namespace Game.Gameplay
{
    public partial class PlayerRoamState : State
    {
        [ExportCategory("State Vars")] 
        [Export] public PlayerInput PlayerInput;

        public override void _Process(double delta)
        {
            GetInputDirection();
            GetInput(delta);
        }

        public void GetInputDirection()
        {
            if (Input.IsActionJustPressed("ui_up"))
            {
                PlayerInput.Direction = Vector2.Up;
                PlayerInput.TargetPosition = new Vector2(0, -16);
            }
            else if (Input.IsActionJustPressed("ui_down"))
            {
                PlayerInput.Direction = Vector2.Down;
                PlayerInput.TargetPosition = new Vector2(0, 16);
            }
            else if (Input.IsActionJustPressed("ui_left"))
            {
                PlayerInput.Direction = Vector2.Left;
                PlayerInput.TargetPosition = new Vector2(-16, 0);
            }
            else if (Input.IsActionJustPressed("ui_right"))
            {
                PlayerInput.Direction = Vector2.Right;
                PlayerInput.TargetPosition = new Vector2(16, 0);
            }
        }

        public void GetInput(double delta)
        {
            if (Input.IsActionJustReleased("ui_up") || Input.IsActionJustReleased("ui_down") ||
                Input.IsActionJustReleased("ui_left") || Input.IsActionJustReleased("ui_right"))
            {
                if (PlayerInput.HoldTime > PlayerInput.HoldThreshold)
                {
                    PlayerInput.EmitSignal(CharacterInput.SignalName.Walk);
                }
                else
                {
                    PlayerInput.EmitSignal(CharacterInput.SignalName.Turn);
                }

                PlayerInput.HoldTime = 0.0f;
            }

            if (Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_down") ||
                Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
            {
                PlayerInput.HoldTime += delta;

                if (PlayerInput.HoldTime > PlayerInput.HoldThreshold)
                {
                    PlayerInput.EmitSignal(CharacterInput.SignalName.Walk);
                }
            }

        }
    }
}
