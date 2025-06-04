using Godot;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.Npc.Ai;

public abstract partial class NpcAiState : State
{
    [Export] public CharacterController Controller;
    protected Npc UserNpc;

    public Vector2 TargetPosition = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
        UserNpc = User as Npc;
    }

    public Vector2 GetDirection()
    {
        return Controller.Direction;
    }

    public virtual bool FoundPlayer()
    {
        return false;
    }

    public virtual bool SeenPlayer()
    {
        return false;
    }
}