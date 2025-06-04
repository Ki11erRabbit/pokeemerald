using Godot;

namespace PokeEmerald.Characters.Npc.Ai;

public partial class WalkToPlayer : NpcAiState
{
    [Export] public CharacterCollisonRayCast PlayerDetector;
    private bool _foundPlayer = false;

    public override void _Ready()
    {
        base._Ready();
        PlayerDetector.Collision += DetectorCollided;
    }

    public void DetectorCollided(bool foundPlayer)
    {
        _foundPlayer = foundPlayer;
    }


    public override bool FoundPlayer()
    {
        return _foundPlayer;
    }

    public override bool SeenPlayer()
    {
        return true;
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckCollision();
        if (_foundPlayer)
        {
            Debug.Log("Player found");
            Machine.TransitionToState("AtPlayer");
        }
    }

    public virtual void CheckCollision()
    {
        if (Controller.Direction == Vector2.Down)
        {
            PlayerDetector.TargetPosition = new Vector2(0, 10);
        } 
        else if (Controller.Direction == Vector2.Up)
        {
            PlayerDetector.TargetPosition = new Vector2(0, -10);
        } 
        else if (Controller.Direction == Vector2.Right)
        {
            PlayerDetector.TargetPosition = new Vector2(10, 0);
        } 
        else if (Controller.Direction == Vector2.Left)
        {
            PlayerDetector.TargetPosition = new Vector2(-10, 0);
        } 
        PlayerDetector.CheckCollision();
    }
}