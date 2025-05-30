using Godot;
using System;

namespace PokeEmerald.Utils.StateMachine;
public partial class StateMachine : Node
{
    [ExportCategory("State Machine Vars")] 
    [Export]
    public Node User;
    [Export] 
    public State CurrentState { get; private set; }

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is not State state)
            {
                continue;
            }
            state.User = User;
            state.SetProcess(false);
        }
    }

    public void ChangeState(State newState)
    {
        if (CurrentState != null) CurrentState.ExitState();
        Debug.Assert(newState != null, "newState != null");
        CurrentState = newState;
        CurrentState.EnterState();
        
        foreach (var child in GetChildren())
        {
            if (child is not State state)
            {
                continue;
            }
            state.SetProcess(child == CurrentState);
            Debug.Log($"Setting processing for {child.Name} to {child == CurrentState}");
        }
    }

    public void TransitionToState(string newState)
    {
        var node = GetNode<State>(newState);
        ChangeState(node);
    }

    public S GetCurrentState<S>() where S : State
    {
        return CurrentState as S;
    }
}
