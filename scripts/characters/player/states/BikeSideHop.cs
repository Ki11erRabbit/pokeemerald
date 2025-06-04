using Godot;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class BikeSideHop : PlayerState
{
    [ExportCategory("Nodes")]
    [Export] public AnimationPlayer AnimationPlayer;
    [Export] public Sprite2D Shadow;
    [Export] public AnimatedSprite2D Dust;
    [ExportCategory("Constants")]
    [Export] public double HoldThreshold = 0.1f;
    private double _holdTime = 0.0;
    private Vector2 _facingDirection = Vector2.Zero;
    [Export] public CharacterCollisonRayCast LedgeRayCast;
    private bool _ledgeColliding = false;

    public override void _Process(double delta)
    {
        if (_ledgeColliding && !Colliding)
        {
            Machine.TransitionToState("BikeRide");
            var newState = Machine.GetCurrentState<CharacterState>();
            Machine.TransitionToState("LedgeJump");
            ResetTargetPosition();
            Machine.GetCurrentState<CharacterState>().SetUp(newState);
            _ledgeColliding = false;
            return;
        }
        base._Process(delta);
    }

    public override void _Ready()
    {
        base._Ready();
        LedgeRayCast.Collision += SetLedgeColliding;
    }
	
    public virtual void SetLedgeColliding(bool colliding, GodotObject what)
    {
        _ledgeColliding = colliding;
    }
    
    
    public override void SetUp(Vector2 direction)
    {
        _facingDirection = direction;
        //Animation.PlayAnimation();
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
		
        LedgeRayCast.EnableCollision();
        RayCast.EnableCollision();
        _ledgeColliding = false;
        Colliding = false;
        CheckCollision();
        
        Shadow.Visible = true;
        Dust.Visible = false;
        AnimationPlayer.AnimationFinished += PlayDustAnimation;
        Dust.AnimationFinished += DustFinished;
        if (AnimationPlayer.IsPlaying()) return;
        AnimationPlayer.Play("wheelie_bounce");
    }

    public override void ExitState()
    {
        base.ExitState();
        AnimationPlayer.AnimationFinished -= PlayDustAnimation;
        LedgeRayCast.DisableCollision();
        RayCast.DisableCollision();
    }

    public override bool IsMoving()
    {
        return true;
    }

    public override double GetMovementSpeed()
    {
        return Globals.Instance.RunningSpeed;
    }

    public override void StartIdling()
    {
        
    }

    public override bool ConfigureAnimationState(AnimatedSprite2D animatedSprite)
    {
        if (_facingDirection == Vector2.Zero)
        {
            SetAnimationState([
                StateMachine.AnimationState.bike_idle_up, 
                StateMachine.AnimationState.bike_idle_left, 
                StateMachine.AnimationState.bike_idle_right, 
                StateMachine.AnimationState.bike_idle_down
            ]);
        }
        else
        {
            if (_facingDirection == Vector2.Up)
            {
                if (Controller.Direction == Vector2.Left)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_up_left;
                }
                else if (Controller.Direction == Vector2.Right)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_up_right;
                }
			    
            }
            else if (_facingDirection == Vector2.Left)
            {
                if (Controller.Direction == Vector2.Up)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_left_up;
                }
                else if (Controller.Direction == Vector2.Down)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_left_down;
                }
            }
            else if (_facingDirection == Vector2.Right)
            {
                if (Controller.Direction == Vector2.Up)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_right_up;
                }
                else if (Controller.Direction == Vector2.Down)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_right_down;
                }
            }
            else if (_facingDirection == Vector2.Down)
            {
                if (Controller.Direction == Vector2.Left)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_down_left;
                }
                else if (Controller.Direction == Vector2.Right)
                {
                    AnimationState = AnimationState.bike_acro_side_hop_down_right;
                }
            }
        }
        return false;
    }
    
    

    private void PlayDustAnimation(StringName _)
    {
        Dust.Visible = true;
        Dust.Play("jump_dust");
        
        Machine.GetState<CharacterState>("BikeIdle").SetUp(_facingDirection);
        Machine.TransitionToState("BikeIdle");
        Shadow.Visible = false;
    }
    
    private void DustFinished()
    {
        Dust.Visible = false;
        Dust.AnimationFinished -= DustFinished;
    }
    
    protected override void CheckCollision()
    {
        base.CheckCollision();
        LedgeRayCast.TargetPosition = Controller.TargetPosition / 2;
        LedgeRayCast.CheckCollision();
    }
}