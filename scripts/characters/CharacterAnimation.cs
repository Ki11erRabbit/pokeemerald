using Godot;
using System;

namespace PokeEmerald.Characters;
public abstract partial class CharacterAnimation : Node2D
{
	[ExportCategory("Nodes")]
	[Export] public CharacterMovement CharacterMovement;
	[Export] public CharacterController CharacterController;
	[Export] public AnimatedSprite2D AnimatedSprite;
	
	public override void _Ready()
	{
		CharacterMovement.Animation += PlayAnimation;
		AnimatedSprite.AnimationFinished += AnimationFinished;
	}

	public abstract void PlayAnimation(string animationType);

	public abstract void AnimationFinished();
}
