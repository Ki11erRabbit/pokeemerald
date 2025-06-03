using Godot;
using System;
using System.Diagnostics;
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
    bike_acro_side_hop_down_left,
    bike_acro_side_hop_down_right,
    bike_acro_side_hop_up_left,
    bike_acro_side_hop_up_right,
    bike_acro_side_hop_right_up,
    bike_acro_side_hop_right_down,
    bike_acro_side_hop_left_up,
    bike_acro_side_hop_left_down,
    
}

public abstract partial class CharacterState : State
{
    [ExportCategory("Nodes")] 
    [Export]
    public CharacterController Controller;
    [Export] public CharacterCollisonRayCast RayCast;
    [Export] public CharacterAnimation Animation;
    [ExportCategory("Vars")] 
    [Export] public Vector2 TargetPosition;
    [Export] public Vector2 StartPosition;
    protected bool Colliding = false;
    public AnimationState AnimationState { get; set; } = AnimationState.idle_down;
    protected AnimationState PreviousAnimation;
    protected Character Character;

    public override void _Process(double delta)
    {
        if (AtTargetPosition() || AtStartPosition())
        {
            SetDirection();
            CheckCollision();
            ProcessPress(delta);
        }
        ProcessBPress(delta);
    }

    
    public virtual void ProcessBPress(double delta)
    {
        
    }

    public override void EnterState()
    {
        StartPosition = Character.Position;
        Animation.PlayAnimation();
        SetTargetPosition();
        RayCast.Collision += SetColliding;
    }

    public override void _Ready()
    {
        Character = User as Character;
    }

    
    public virtual void SetColliding(bool colliding, GodotObject what)
    {
        Colliding = colliding;
    }
    
    public virtual void SetUp(CharacterState _)
    {
        
    }
    
    public virtual void SetUp(bool _)
    {
        
    }
    public virtual void SetUp(Vector2 _)
    {
        
    }
    
    public void Animate(AnimatedSprite2D animatedSprite)
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
    
    public abstract double GetMovementSpeed();

    public virtual void Move(double delta)
    {
        CheckCollision();
        if (Colliding && AtStartPosition())
        {
            ResetTargetPosition();
            return;
        }
        delta *= Globals.Instance.TileSize * GetMovementSpeed();
        Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);
    }

    public abstract bool IsMoving();

    public abstract void StartIdling();

    public virtual void SetTargetPosition()
    {
        StartPosition = Character.Position;
        TargetPosition = Character.Position + Controller.Direction * Globals.Instance.TileSize;
    }

    public void ResetTargetPosition()
    {
        TargetPosition = Character.Position;
    }

    public bool AtTargetPosition()
    {
        return Character.Position.DistanceTo(TargetPosition) < 0.3f;
    }

    public bool AtStartPosition()
    {
        return Character.Position.IsEqualApprox(StartPosition);
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
        
        if (Input.IsActionJustPressed("ui_up"))
        {
            Controller.Direction = Vector2.Up;
            Controller.TargetPosition = new Vector2(0, -16);
        }
        else if (Input.IsActionJustPressed("ui_down"))
        {
            Controller.Direction = Vector2.Down;
            Controller.TargetPosition = new Vector2(0, 16);
        } 
        if (Input.IsActionJustPressed("ui_left"))
        {
            Controller.Direction = Vector2.Left;
            Controller.TargetPosition = new Vector2(-16, 0);
        }
        else if (Input.IsActionJustPressed("ui_right"))
        {
            Controller.Direction = Vector2.Right;
            Controller.TargetPosition = new Vector2(16, 0);
        }
    }

    protected virtual void ProcessPress(double delta)
    {
        
    }

    protected virtual void CheckCollision()
    {
        RayCast.TargetPosition = Controller.TargetPosition;
        RayCast.CheckCollision();
    }

    public void SnapToGrid()
    {
        TargetPosition = Globals.Instance.SnapToGrid(Character.Position);
    }
    
    protected virtual void CheckForPositionAndCollison(IReachedTargetPosition method)
    {
        if (AtTargetPosition() && !Colliding)
        {
            method.Colliding(this);
        }
        else if (AtTargetPosition() && Colliding)
        {
            method.NotColliding(this);
        }
    }
    
    protected interface IReachedTargetPosition
    {
        public void Colliding(CharacterState self);
        public void NotColliding(CharacterState self);
    }
}
