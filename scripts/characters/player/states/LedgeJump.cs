using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class LedgeJump : PlayerState
{
    [Export] public AnimatedSprite2D Sprite;
    [Export] public Sprite2D Shadow;
    [Export] public AnimatedSprite2D Dust;
    private CharacterState _lastState;
    private double _timeElapsed = 0;
    public override void _Process(double delta)
    {
        base._Process(delta);
        AnimateSprite(delta);
    }

    private void AnimateSprite(double delta)
    {
        _timeElapsed += delta / delta;
        if (AtTargetPosition())
        {
            var position = Sprite.Position;
            Sprite.Position = new Vector2(position.X, 4);
            Machine.ChangeState(_lastState);
            _lastState.ResetTargetPosition();
            Dust.Visible = true;
            Dust.Play("jump_dust");
            return;
        }

        var y = Mathf.Floor(4 - 1.5 * _timeElapsed + 0.0468 * Mathf.Pow(_timeElapsed, 2));
        var pos = Sprite.Position;
        Sprite.Position = new Vector2(pos.X, (float) y);
    }

    public override void EnterState()
    {
        base.EnterState();
        Dust.AnimationFinished += DustFinished;
        Shadow.Visible = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        _timeElapsed = 0;
        Shadow.Visible = false;
    }

    public override void SetUp(CharacterState state)
    {
        _lastState = state;
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        animatedSprite.Pause();
        return true;
    }

    public override double GetMovementSpeed()
    {
        return Globals.Instance.WalkingSpeed;
    }

    public override bool IsMoving()
    {
        return true;
    }

    public override void StartIdling()
    {
        _lastState.StartIdling();
    }

    public override void SetTargetPosition()
    {
        StartPosition = Character.Position;
        TargetPosition = Character.Position + Controller.Direction * (Globals.Instance.TileSize * 2);
        Debug.Log($"Added position {Controller.Direction * (Globals.Instance.TileSize * 2)}");
        Debug.Log($"Target Position: {TargetPosition}");
        Debug.Log($"Normal TargetPosition: {Character.Position + Controller.Direction * Globals.Instance.TileSize}");
    }
    
    public override void Move(double delta)
    {
        delta *= Globals.Instance.TileSize * GetMovementSpeed();
        Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);
        Debug.Log($"Position: {Character.Position}");
    }
    
    private void DustFinished()
    {
        Dust.Visible = false;
        Dust.AnimationFinished -= DustFinished;
    }
}