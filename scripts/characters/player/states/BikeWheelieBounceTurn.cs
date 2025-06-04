using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeWheelieBounceTurn : PlayerState
{
    [ExportCategory("Nodes")]
    [Export] public AnimationPlayer AnimationPlayer;
    [Export] public Sprite2D Shadow;
    [Export] public AnimatedSprite2D Dust;
    private CharacterState _previousState;

    public override void _Process(double delta)
    {
        Machine.ChangeState(_previousState);
    }
    
    
    public override void SetUp(CharacterState input)
    {
        _previousState = input;
    }

    private void StopAnimation()
    {
        AnimationPlayer.Stop();
        AnimationPlayer.Play("RESET");
        Shadow.Visible = false;
        Dust.Play("default");
        Dust.Visible = false;
    }

    public override void EnterState()
    {
        base.EnterState();
        Shadow.Visible = true;
        Dust.Visible = true;
        AnimationPlayer.AnimationFinished += PlayDustAnimation;
        Dust.AnimationFinished += DustFinished;
        if (AnimationPlayer.IsPlaying()) return;
        AnimationPlayer.Play("wheelie_bounce");
    }

    public override void ExitState()
    {
        base.ExitState();
        AnimationPlayer.AnimationFinished -= PlayDustAnimation;
        Dust.AnimationFinished -= DustFinished;
    }

    public override bool IsMoving()
    {
        return false;
    }

    public override double GetMovementSpeed()
    {
        return 0;
    }

    public override void StartIdling()
    {
        Machine.TransitionToState("BikeWheelieBounceIdle");
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        SetAnimationState([
            StateMachine.AnimationState.bike_acro_wheelie_idle_up, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_left, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_right, 
            StateMachine.AnimationState.bike_acro_wheelie_idle_down
        ]);
        return false;
    }
    

    private void PlayDustAnimation(StringName _)
    {
        Dust.Visible = true;
        Dust.Play("jump_dust");
        AnimationPlayer.Play("wheelie_bounce");
    }
    
    private void DustFinished()
    {
        Dust.Visible = false;
    }
}