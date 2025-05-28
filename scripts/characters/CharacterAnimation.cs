using Godot;
using System;

namespace PokeEmerald.Characters;
public abstract partial class CharacterAnimation : Node2D
{
	[ExportCategory("Nodes")]
	[Export] public CharacterMovement CharacterMovement;
	[Export] public CharacterController CharacterController;
	
	public override void _Ready()
	{
		CharacterMovement.Animation += PlayAnimation;
	}

	public abstract void PlayAnimation(string animationType);
}
