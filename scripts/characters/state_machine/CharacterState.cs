using Godot;
using System;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.StateMachine;

public enum AnimationState
{
    idle_down,
    idle_up,
    idle_left,
    idle_right,
    turn_down,
    turn_up,
    turn_left,
    turn_right,
    walk_down,
    walk_up,
    walk_left,
    walk_right,
    run_down,
    run_up,
    run_left,
    run_right,
    bike_idle_down,
    bike_idle_up,
    bike_idle_left,
    bike_idle_right,
    bike_turn_down,
    bike_turn_up,
    bike_turn_left,
    bike_turn_right,
    bike_acro_ride_down,
    bike_acro_ride_up,
    bike_acro_ride_left,
    bike_acro_ride_right,
    bike_mach_ride_down,
    bike_mach_ride_up,
    bike_mach_ride_left,
    bike_mach_ride_right,
    bike_acro_wheelie_idle_down,
    bike_acro_wheelie_idle_up,
    bike_acro_wheelie_idle_left,
    bike_acro_wheelie_idle_right,
    bike_acro_wheelie_ride_down,
    bike_acro_wheelie_ride_up,
    bike_acro_wheelie_ride_left,
    bike_acro_wheelie_ride_right,
    bike_acro_wheelie_start_down,
    bike_acro_wheelie_start_up,
    bike_acro_wheelie_start_left,
    bike_acro_wheelie_start_right,
    bike_acro_wheelie_end_down,
    bike_acro_wheelie_end_up,
    bike_acro_wheelie_end_left,
    bike_acro_wheelie_end_right,
    bike_acro_wheelie_turn_down,
    bike_acro_wheelie_turn_up,
    bike_acro_wheelie_turn_left,
    bike_acro_wheelie_turn_right,
}

public abstract partial class CharacterState : State
{
    [ExportCategory("Nodes")] 
    [Export]
    public CharacterController Controller;
    [Export] public CharacterAnimation Animation;
    [ExportCategory("Vars")] 
    [Export] public Vector2 TargetPosition;
    public AnimationState AnimationState { get; set; } = AnimationState.idle_down;
    protected AnimationState _previousAnimation;
    protected Character _character;

    public override void EnterState()
    {
        Animation.PlayAnimation();
        SetTargetPosition();
        Enter();
    }

    public virtual void Enter()
    {
        
    }

    public override void _Ready()
    {
        _character = User as Character;
        CustomReady();
    }

    public virtual void CustomReady()
    {
        
    }
    
    public virtual void SetUp(CharacterState _)
    {
        
    }
    
    public virtual void SetUp(bool _)
    {
        
    }
    
    public void animate(AnimatedSprite2D animatedSprite)
    {
        Debug.Log("Configuring Animation State");
        if (ConfigureAnimationState(animatedSprite)) return;
        Debug.Log("Playing animation: " + AnimationState.ToString());
        animatedSprite.Play(AnimationState.ToString());
    }

    public abstract bool ConfigureAnimationState(AnimatedSprite2D animatedSprite);
    
    protected void SetAnimationState(AnimationState[] possibleStates)
    {
        if (Controller.Direction == Vector2.Up)
        {
            AnimationState = possibleStates[0];
        }
        else if (Controller.Direction == Vector2.Left)
        {
            AnimationState = possibleStates[1];
        }
        else if (Controller.Direction == Vector2.Right)
        {
            AnimationState = possibleStates[2];
        }
        else if (Controller.Direction == Vector2.Down)
        {
            AnimationState = possibleStates[3];
        }
    }

    public abstract void Move(double delta);

    public abstract bool IsMoving();

    public abstract void StartIdling();

    public void SetTargetPosition()
    {
        TargetPosition = _character.Position + Controller.Direction * Globals.Instance.TileSize;
    }

    public bool AtTargetPosition()
    {
        return _character.Position.DistanceTo(TargetPosition) < 0.8f;
    }
    
    protected virtual void SetDirection()
    {
        if (Input.IsActionPressed("ui_up"))
        {
            Controller.Direction = Vector2.Up;
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (Input.IsActionPressed("ui_down"))
        {
            Controller.Direction = Vector2.Down;
            Controller.TargetPosition = new Vector2(0, 16);
        } 
        if (Input.IsActionPressed("ui_left"))
        {
            Controller.Direction = Vector2.Left;
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            Controller.Direction = Vector2.Right;
            Controller.TargetPosition = new Vector2(16, 0);
        }
    }

    public void SnapToGrid()
    {
        TargetPosition = Globals.Instance.SnapToGrid(_character.Position);
    }
}
