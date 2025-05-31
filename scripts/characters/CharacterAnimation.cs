using Godot;
using System;
using PokeEmerald.Characters.StateMachine;

namespace PokeEmerald.Characters;
public partial class CharacterAnimation : Node2D
{
	[ExportCategory("Nodes")]
	[Export] public CharacterMovement CharacterMovement;
	[Export] public CharacterController CharacterController;
	[Export] public AnimatedSprite2D AnimatedSprite;
	[Export] public Utils.StateMachine.StateMachine StateMachine;
	
	public override void _Ready()
	{
		CharacterMovement.Animation += PlayAnimation;
		CustomReady();
	}

	public void PlayAnimation()
	{
		StateMachine.GetCurrentState<CharacterState>().Animate(AnimatedSprite);
	}

	public virtual void CustomReady()
	{
		
	}
	
}
