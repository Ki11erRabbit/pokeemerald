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
        if (newState == null)
        {
            GD.PrintErr("newState is null");
            return;
        }
        CurrentState = newState;
        CurrentState.EnterState();
        
        foreach (var child in GetChildren())
        {
            if (child is not State state)
            {
                continue;
            }
            state.SetProcess(child == CurrentState);
        }
    }
}
