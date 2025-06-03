using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public abstract partial class PlayerIdleState : CharacterState
{
    [ExportCategory("Vars")] 
    [Export] public double HoldThreshold = 0.1f;
    protected bool SameDirection = false;
    protected double HoldTime = 0;
    
    public override void ExitState()
    {
        HoldTime = 0;
        SameDirection = false;
    }
    
    public override double GetMovementSpeed()
    {
        return 0;
    }

    public override void StartIdling()
    {
		
    }
    
    public override bool IsMoving()
    {
        return false;
    }
}