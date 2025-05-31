using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public abstract partial class BikeWheelieTransitionState : CharacterState
{
    [Export] public AnimatedSprite2D AnimatedSprite;
    [Export] public double FrameThreshold = 0.3;
    protected double _frameTime = 0.0;

    public override void EnterState()
    {
        base.EnterState();
        SetStartFrame();
        AnimatedSprite.Play(AnimationState.ToString());
    }

    protected bool CheckForEnd(double delta)
    {
        _frameTime += delta;
        if (_frameTime >= FrameThreshold)
        {
            SetEndFrame();
            return true;
        }
        else
        {
            SetStartFrame();
        }
        
        return false;
    }

    public override void ExitState()
    {
        base.ExitState();
        _frameTime = 0.0;
    }

    public override void SetUp(CharacterState state)
    {
        TargetPosition = state.TargetPosition;
    }
    
    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        if (_frameTime >= FrameThreshold)
        {
            SetEndFrame();
        }
        else
        {
            SetStartFrame();
        }
        return false;
    }

    protected virtual void SetStartFrame()
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_start_up, 
            StateMachine.AnimationState.bike_acro_wheelie_start_left, 
            StateMachine.AnimationState.bike_acro_wheelie_start_right, 
            StateMachine.AnimationState.bike_acro_wheelie_start_down
        ]);
    }
    protected virtual void SetEndFrame()
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_end_up, 
            StateMachine.AnimationState.bike_acro_wheelie_end_left, 
            StateMachine.AnimationState.bike_acro_wheelie_end_right, 
            StateMachine.AnimationState.bike_acro_wheelie_end_down
        ]);
    }
}