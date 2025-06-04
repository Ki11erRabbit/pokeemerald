using Godot;
using PokeEmerald.Characters.Npc.Ai;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Npc.States;

public abstract partial class NpcState : CharacterState
{
    [Export] public Utils.StateMachine.StateMachine AiStateMachine;

    protected override void SetDirection()
    {
        var direction = AiStateMachine.GetCurrentState<NpcAiState>().GetDirection();
        
        Controller.Direction = direction;
        if (direction == Vector2.Up)
        {
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (direction == Vector2.Down)
        {
            Controller.TargetPosition = new Vector2(0, 16);
        } 
        if (direction == Vector2.Left)
        {
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (direction == Vector2.Right)
        {
            Controller.TargetPosition = new Vector2(16, 0);
        }
    }
    
    public override void _Process(double delta)
    {
        if (AtTargetPosition() || AtStartPosition())
        {
            SetDirection();
            CheckCollision();
            ProcessAi(delta);
        }
    }
    
    public abstract void ProcessAi(double delta);
}