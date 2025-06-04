using Godot;

namespace PokeEmerald.GameState;

public partial class TransientGameState : Node
{
    public static TransientGameState Instance { get; private set; }
    [ExportCategory("Flags")]
    [Export] public bool Running { get; set; } = false;
    [Export] public bool CanCharacterMove { get; set; } = true;

    public override void _Ready()
    {
        Instance = this;
    }

    public static void SetCharacterRunning(bool running)
    {
        Instance.CanCharacterMove = running;
    }
    public static bool IsCharacterRunning()
    {
        return Instance.Running;
    }

    public static void SetCanCharacterMove(bool canCharacterMove)
    {
        Instance.CanCharacterMove = canCharacterMove;
    }

    public static bool CanMove()
    {
        return Instance.CanCharacterMove;
    }
    
}