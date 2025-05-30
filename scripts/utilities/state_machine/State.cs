using Godot;
using System;
namespace PokeEmerald.Utils.StateMachine;
public partial class State : Node
{
    [Export] public Node User;
    [Export] public StateMachine Machine;

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }
}
