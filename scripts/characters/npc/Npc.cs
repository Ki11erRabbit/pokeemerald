using Godot;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.Npc;

public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
}

public partial class Npc : Character
{
    [Export] public string StartingAiState;
    [ExportCategory("Nodes")]
    [Export] public CharacterCollisonRayCast SightRayCast { get; set; }
    [Export] public Utils.StateMachine.StateMachine AiStateMachine { get; set; }
    [ExportCategory("Vars")]
    [Export] public ulong SightRange { get; set; }

    [Export] public FacingDirection StartingDirection = FacingDirection.Down;

    

    public override void _Ready()
    {
        base._Ready();
        
        switch (StartingDirection)
        {
            case FacingDirection.Down:
                CharacterController.Direction = Vector2.Down;
                break;
            case FacingDirection.Up:
                CharacterController.Direction = Vector2.Up;
                break;
            case FacingDirection.Right:
                CharacterController.Direction = Vector2.Right;
                break;
            case FacingDirection.Left:
                CharacterController.Direction = Vector2.Left;
                break;
        }
        
        SetSightRayCast();
        
        var state = StateMachine.GetNode<State>(StartingAiState);
        AiStateMachine.ChangeState(state);
        AiStateMachine.TransitionToState(StartingAiState);
        
        
    }

    public void SetSightRayCast()
    {
        var targetPosition = CharacterController.Direction;
        if (CharacterController.Direction == Vector2.Up)
        {
            targetPosition = new Vector2(
                targetPosition.X * Globals.Instance.TileSize * SightRange,
                targetPosition.Y * Globals.Instance.TileSize * SightRange);
        }
        else if (CharacterController.Direction == Vector2.Down)
        {
            targetPosition = new Vector2(
                targetPosition.X * Globals.Instance.TileSize * SightRange,
                targetPosition.Y * Globals.Instance.TileSize * SightRange);
        }
        else if (CharacterController.Direction == Vector2.Left)
        {
            targetPosition = new Vector2(
                targetPosition.X * Globals.Instance.TileSize * SightRange,
                targetPosition.Y * Globals.Instance.TileSize * SightRange);
        }
        else if (CharacterController.Direction == Vector2.Right)
        {
            targetPosition = new Vector2(
                targetPosition.X * Globals.Instance.TileSize * SightRange,
                targetPosition.Y * Globals.Instance.TileSize * SightRange);
        }
        SightRayCast.TargetPosition = targetPosition;
    }
}