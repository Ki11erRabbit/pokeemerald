using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Turn : CharacterState
{
    private CharacterState _previousState;
    public override void _Process(double delta)
    {
        Machine.ChangeState(_previousState);
    }
    public override void Move(double delta)
    {
        
    }
    
    public override bool IsMoving()
    {
        return false;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("Idle");
    }

    public override void SetUp(CharacterState input)
    {
        _previousState = input;
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.turn_up, 
            StateMachine.AnimationState.turn_left, 
            StateMachine.AnimationState.turn_right, 
            StateMachine.AnimationState.turn_down
        ]);
        return false;
    }
    
}