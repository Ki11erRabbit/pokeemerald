using Godot;
using System;

namespace PokeEmerald.Characters;
public partial class CharacterController : Node
{
	[Signal]
	public delegate void IdleEventHandler();
	[Signal]
	public delegate void WalkEventHandler();
	[Signal]
	public delegate void TurnEventHandler();
	[Signal]
	public delegate void RunEventHandler();
	[Signal]
	public delegate void CycleEventHandler();
	[Signal]
	public delegate void CycleStopEventHandler();
	[Signal]
	public delegate void SwimmingEventHandler();
	[Signal]
	public delegate void JumpingEventHandler();
	[Signal]
	public delegate void DivingEventHandler();
	[ExportCategory("Common Input")]
	[Export] public Vector2 Direction = Vector2.Zero;
	[Export] public Vector2 TargetPosition = Vector2.Zero;
	[ExportCategory("Movement")]
	[Export] public CharacterMovement Movement;
	
}
