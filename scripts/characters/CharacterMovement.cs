using Godot;
using System;

namespace PokeEmerald.Characters;

public enum MovingState
{
    Idle,
    Walking,
    Running,
}

public enum MovementState
{
    Grounded,
    Cycling,
    Swimming,
    Jumping,
    Diving,
}

public partial class CharacterMovement : Node
{
    [Signal]
    public delegate void AnimationEventHandler(string animationType);

    [ExportCategory("Nodes")] 
    [Export] public Character Character;
    [Export] public CharacterController CharacterController;
    [ExportCategory("Movement")] 
    [Export] public Vector2 TargetPosition = Vector2.Down;
    [Export] public MovingState CurrentMovingState = MovingState.Idle;
    [Export] public MovementState CurrentMovementState = MovementState.Grounded;

    public override void _Ready()
    {
        Character = GetParent<Character>();
        CharacterController.Idle += StartIdling;
        CharacterController.Walk += StartWalking;
        CharacterController.Run += StartRunning;
        CharacterController.Cycle += StartCycling;
        CharacterController.CycleStop += StopCycling;
        CharacterController.Swimming += StartSwimming;
        CharacterController.Jumping += StartJumping;
        CharacterController.Diving += StartDiving;
        CharacterController.Turn += Turn;
        SnapPositionToGrid();
    }

    public override void _Process(double delta)
    {
        Move(delta);
    }

    public bool IsMoving()
    {
        return CurrentMovingState switch
        {
            MovingState.Idle => false,
            _ => true
        };
    }

    private bool IsWalking()
    {
        var walking = IsMoving() && CurrentMovementState switch
        {
            MovementState.Grounded => true,
            _ => false,
        } && CurrentMovingState == MovingState.Walking;
        return walking;
    }

    private bool IsRunning()
    {
        return CurrentMovementState switch
        {
            MovementState.Grounded => true,
            _ => false,
        } && CurrentMovingState == MovingState.Running;
    }

    private bool IsCycling()
    {
        return CurrentMovementState switch
        {
            MovementState.Cycling => true,
            _ => false,
        };
    }
    
    private bool IsSwimming()
    {
        return CurrentMovementState switch
        {
            MovementState.Swimming => true,
            _ => false,
        };
    }
    
    private bool IsJumping()
    {
        return CurrentMovementState switch
        {
            MovementState.Jumping => true,
            _ => false,
        };
    }
    
    private bool IsDiving()
    {
        return CurrentMovementState switch
        {
            MovementState.Diving => true,
            _ => false,
        };
    }

    public void StartWalking()
    {
        if (IsMoving()) return;
        EmitSignal(SignalName.Animation, "walk");
        TargetPosition = Character.Position + CharacterController.Direction * Globals.Instance.TileSize;
        CurrentMovingState = MovingState.Walking;
    }

    public void StartIdling()
    {
        EmitSignal(SignalName.Animation, "idle");
        CurrentMovingState = MovingState.Idle;
        SnapPositionToGrid();
    }

    public void StartRunning()
    {
        if (IsMoving()) return;
        EmitSignal(SignalName.Animation, "run");
        TargetPosition = Character.Position + CharacterController.Direction * Globals.Instance.TileSize;
        CurrentMovingState = MovingState.Running;
    }

    public void StartCycling()
    {
        if (IsCycling()) return;
        EmitSignal(SignalName.Animation, "cycle");
        TargetPosition = Character.Position;
        CurrentMovementState = MovementState.Cycling;
        CurrentMovingState = MovingState.Walking;
    }
    public void StopCycling()
    {
        if (!IsCycling()) return;
        EmitSignal(SignalName.Animation, "idle");
        TargetPosition = Character.Position + CharacterController.Direction * Globals.Instance.TileSize;
        CurrentMovementState = MovementState.Grounded;
    }

    public void StartSwimming()
    {
        if (IsSwimming()) return;
        EmitSignal(SignalName.Animation, "swim");
        // TODO: fetch direction from character controller
        CurrentMovementState = MovementState.Swimming;
    }

    public void StartJumping()
    {
        if(IsJumping()) return;
        EmitSignal(SignalName.Animation, "jump");
        // TODO: fetch direction from character controller
        CurrentMovementState = MovementState.Jumping;
    }

    public void StartDiving()
    {
        if(IsDiving()) return;
        EmitSignal(SignalName.Animation, "dive");
        // TODO: fetch direction from character controller
        CurrentMovementState = MovementState.Diving;
    }
    
    public void Turn()
    {
        EmitSignal(SignalName.Animation, "turn");
    }
    
    public void Move(double delta)
    {
        if (IsWalking())
        {
            delta *= Globals.Instance.TileSize * Globals.Instance.WalkingSpeed;
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);

        }
        else if (IsRunning())
        {
            delta *= Globals.Instance.TileSize * Globals.Instance.RunningSpeed;
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);
        }
        else if (IsCycling())
        {
            delta *= Globals.Instance.TileSize * Globals.Instance.GetBikeSpeed();
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);

        }
        else if (IsJumping())
        {
            if (CurrentMovingState == MovingState.Running)
            {
                if (CurrentMovementState == MovementState.Cycling)
                {
                    delta *= Globals.Instance.TileSize * Globals.Instance.GetBikeSpeed();
                }
                else
                {
                    delta *= Globals.Instance.TileSize * Globals.Instance.RunningSpeed;
                }
            }
            else if (CurrentMovingState == MovingState.Walking)
            {
                delta *= Globals.Instance.TileSize * Globals.Instance.WalkingSpeed;
            }
            else
            {
                Debug.Assert(true, "Invalid moving state");
            }
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);
            // TODO: Animate Sprite while jumping
        }
        else if (IsSwimming())
        {
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);// TODO: add tilesize constant and swim rate

        }
        else if (IsDiving())
        {
            Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta);// TODO: add tilesize constant and dive rate

        }
        if (Character.Position.DistanceTo(TargetPosition) < 1f)
        {
            StartIdling();
        }
    }
    public void SnapPositionToGrid()
    {
        Globals.Instance.SnapToGrid(Character.Position);
    }
}
