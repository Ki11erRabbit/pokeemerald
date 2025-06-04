using Godot;
using PokeEmerald.Characters.Npc.Ai;

namespace PokeEmerald.Characters.Npc.States;

public partial class Walk : NpcState
{
    public override bool IsMoving()
    {
        return true;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("Idle");
    }
    

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.walk_up, 
            StateMachine.AnimationState.walk_left, 
            StateMachine.AnimationState.walk_right, 
            StateMachine.AnimationState.walk_down
        ]);
        return false;
    }

    public override double GetMovementSpeed()
    {
        return Globals.Instance.WalkingSpeed;
    }
    
    public override void ProcessAi(double delta)
    {
        if (!AiStateMachine.GetCurrentState<NpcAiState>().FoundPlayer())
        {
            Machine.TransitionToState("Idle");
        }
        EnterState();
    }
}