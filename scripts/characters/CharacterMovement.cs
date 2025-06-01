using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters;



public partial class CharacterMovement : Node
{
    [Signal]
    public delegate void AnimationEventHandler();

    [ExportCategory("Nodes")] 
    [Export] public Character Character;
    [Export] public CharacterController CharacterController;
    [Export] public Utils.StateMachine.StateMachine StateMachine;
    [ExportCategory("Vars")] 
    [Export] public bool CollisionDetected = false;

    public override void _Ready()
    {
        Character = GetParent<Character>();
        SnapPositionToGrid();
    }

    public override void _Process(double delta)
    {
        Move(delta);
    }

    public void SetColliding(bool value)
    {
        Debug.Log("Collision detected");
        CollisionDetected = value;
    }

    public bool IsColliding()
    {
        return CollisionDetected;
    }

    
    public void Move(double delta)
    {
        var currentState = StateMachine.GetCurrentState<CharacterState>();
        if (currentState.IsMoving())
        {
            currentState.Move(delta);
        }
        
        if (currentState.AtTargetPosition())
        {
            SnapPositionToGrid();
            //currentState.StartIdling();
        }
    }
    public void SnapPositionToGrid()
    {
        Character.Position = Globals.Instance.SnapToGrid(Character.Position);
        Character.StateMachine.GetCurrentState<CharacterState>().SnapToGrid();
    }
}
