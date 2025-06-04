using Godot;
using PokeEmerald.Characters.Npc.Ai;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Npc.States;

public partial class Idle : NpcState
{
    public override void _Ready()
    {
        base._Ready();
        var direction = (User as Npc).StartingDirection;
        switch (direction)
        {
            case FacingDirection.Down:
                Controller.Direction = Vector2.Down;
                break;
            case FacingDirection.Up:
                Controller.Direction = Vector2.Up;
                break;
            case FacingDirection.Right:
                Controller.Direction = Vector2.Right;
                break;
            case FacingDirection.Left:
                Controller.Direction = Vector2.Left;
                break;
        }
    }

    public override bool IsMoving()
    {
        return false;
    }

    public override void StartIdling()
    {
        
    }
    

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.idle_up, 
            StateMachine.AnimationState.idle_left, 
            StateMachine.AnimationState.idle_right, 
            StateMachine.AnimationState.idle_down
        ]);
        return false;
    }

    public override double GetMovementSpeed()
    {
        return 0;
    }

    public override void ProcessAi(double delta)
    {
        
        if (AiStateMachine.GetCurrentState<NpcAiState>().SeenPlayer())
        {
            Debug.Log("seeing player");
            Machine.TransitionToState("Walk");
        }
    }
}