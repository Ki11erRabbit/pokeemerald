using Godot;
using System;
using PokeEmerald.Utils.StateMachine;

namespace PokeEmerald.Characters.Player.States;

public partial class Walk : State
{
	[ExportCategory("Controller")] 
	[Export]
	public PlayerController Controller;
	public override void _Process(double delta)
	{
		Controller.SetDirection();
		Controller.ProcessPress(delta);
	}
}
