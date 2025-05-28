using Godot;
using System;

namespace Game.Gameplay
{
	public partial class CharacterMovement : Node
	{
		[Signal]
		public delegate void AnimationEventHandler(string animationType);

		[ExportCategory("Nodes")] 
		[Export] public Node2D Character;

		[Export] public CharacterInput CharacterInput;
		[ExportCategory("Movement")]
		[Export] public Vector2 TargetPosition = Vector2.Down;

		[Export] public bool IsWalking = false;
		public override void _Ready()
		{
			CharacterInput.Walk += StartWalking;
			CharacterInput.Turn += Turn;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			Walk(delta);
		}

		public bool IsMoving()
		{
			return IsWalking;
		}
		
		public void StartWalking()
		{
			if (!IsMoving())
			{
				EmitSignal(SignalName.Animation, "walk");
				TargetPosition = Character.Position + CharacterInput.Direction * 16;
				IsWalking = true;
			}
		}

		public void Walk(double delta)
		{
			if (IsWalking)
			{
				Character.Position = Character.Position.MoveToward(TargetPosition, (float)delta * 16 * 4);

				if (Character.Position.DistanceTo(TargetPosition) < 1f)
				{
					StopWalking();
				}
			}
			else
			{
				EmitSignal(SignalName.Animation, "idle");
				
			}
		}

		public void StopWalking()
		{
			IsWalking = false;
			
		}
		
		public void Turn()
		{
			EmitSignal(SignalName.Animation, "turn");
		}
		
		public void SnapPositionToGrid()
		{
			// TODO: fetch grid size from global
			Character.Position = new Vector2(
				Mathf.Round(Character.Position.X / 16) * 16,
				Mathf.Round(Character.Position.Y / 16) * 16
				);
		}
	}
}

