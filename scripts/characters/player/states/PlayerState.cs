using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public abstract partial class PlayerState : CharacterState
{
    
    protected override void SetDirection()
    {
        if (Input.IsActionPressed("ui_up"))
        {
            Controller.Direction = Vector2.Up;
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Controller.Direction = Vector2.Down;
            Controller.TargetPosition = new Vector2(0, 16);
        } 
        if (Input.IsActionPressed("ui_left"))
        {
            Controller.Direction = Vector2.Left;
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            Controller.Direction = Vector2.Right;
            Controller.TargetPosition = new Vector2(16, 0);
        }
        
        if (Input.IsActionJustPressed("ui_up"))
        {
            Controller.Direction = Vector2.Up;
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            Controller.Direction = Vector2.Down;
            Controller.TargetPosition = new Vector2(0, 16);
        } 
        if (Input.IsActionJustPressed("ui_left"))
        {
            Controller.Direction = Vector2.Left;
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (Input.IsActionJustPressed("ui_right"))
        {
            Controller.Direction = Vector2.Right;
            Controller.TargetPosition = new Vector2(16, 0);
        }
    }
}