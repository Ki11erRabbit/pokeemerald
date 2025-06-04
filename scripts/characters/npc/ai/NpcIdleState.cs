using Godot;

namespace PokeEmerald.Characters.Npc.Ai;



public abstract partial class NpcIdleState : NpcAiState
{
    [Export] public CharacterCollisonRayCast SightRayCast { get; set; }
    private bool _seenPlayer = false;

    public override void _Ready()
    {
        base._Ready();
        SightRayCast.Collision += SightCollided;
    }
    

    public void SightCollided(bool seenPlayer)
    {
        _seenPlayer = seenPlayer;
        GameState.TransientGameState.SetCanCharacterMove(false);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckCollision();
        if (_seenPlayer)
        {
            Debug.Log("Player seen");
            Machine.TransitionToState("WalkToPlayer");
            Machine.GetCurrentState<NpcAiState>().TargetPosition = SightRayCast.TargetPosition;
        }
    }

    public override bool SeenPlayer()
    {
        return _seenPlayer;
    }

    public virtual void CheckCollision()
    {
        UserNpc.SetSightRayCast();
        SightRayCast.CheckCollision();
    }
}