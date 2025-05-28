using Godot;
using System;

namespace PokeEmerald.Characters.Player;

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
}

public partial class PlayerAnimation : CharacterAnimation
{
	[ExportCategory("Animation Vars")] 
	[Export]
	public AnimationState AnimationState = AnimationState.idle_down;

	[Export] public AnimatedSprite2D WalkSprites;
	public override void PlayAnimation(string animationType)
	{
		var previousAnimation = AnimationState;

		if (CharacterMovement.IsMoving()) return;

		switch (animationType)
		{
			case "walk":
				SetAnimationState([AnimationState.walk_up, AnimationState.walk_left, AnimationState.walk_right, AnimationState.walk_down]);
				break;
			case "turn":
				SetAnimationState([AnimationState.turn_up, AnimationState.turn_left, AnimationState.turn_right, AnimationState.turn_down]);
				break;
			case "idle":
				// TODO: handle idling on bike, swimming/surfing, diving
				SetAnimationState([AnimationState.idle_up, AnimationState.idle_left, AnimationState.idle_right, AnimationState.idle_down]);
				break;
		}

		if (previousAnimation != AnimationState)
		{
			WalkSprites.Play(AnimationState.ToString());
		}
	}

	private void SetAnimationState(AnimationState[] possibleStates)
	{
		if (CharacterController.Direction == Vector2.Up)
		{
			AnimationState = possibleStates[0];
		}
		else if (CharacterController.Direction == Vector2.Left)
		{
			AnimationState = possibleStates[1];
		}
		else if (CharacterController.Direction == Vector2.Right)
		{
			AnimationState = possibleStates[2];
		}
		else if (CharacterController.Direction == Vector2.Down)
		{
			AnimationState = possibleStates[3];
		}
	}
}
