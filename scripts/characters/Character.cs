using Godot;
using System;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters;

public partial class Character : CharacterBody2D
{
	
	[ExportCategory("MovementControl")] 
	[Export] public CharacterController CharacterController;

	[Export] public Utils.StateMachine.StateMachine StateMachine;
	[Export] public string StartingState;

	public override void _Ready()
	{
		var state = StateMachine.GetNode<State>(StartingState);
		StateMachine.ChangeState(state);
	}
}
