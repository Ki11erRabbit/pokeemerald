using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieTurn : CharacterState
{
    private CharacterState _previousState;
    public override void _Process(double delta)
    {
        Machine.ChangeState(_previousState);
    }
    public override double GetMovementSpeed()
    {
        return 0;
    }
    
    public override bool IsMoving()
    {
        return false;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("BikeWheelieIdle");
    }

    public override void SetUp(CharacterState input)
    {
        _previousState = input;
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_turn_up, 
            StateMachine.AnimationState.bike_acro_wheelie_turn_left, 
            StateMachine.AnimationState.bike_acro_wheelie_turn_right, 
            StateMachine.AnimationState.bike_acro_wheelie_turn_down
        ]);
        return false;
    }
    
}