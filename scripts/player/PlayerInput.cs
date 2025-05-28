using Godot;
using System;

namespace Game.Gameplay
{
	public partial class PlayerInput : CharacterInput
	{
		[ExportCategory("Player Input")] [Export]
		public double HoldThreshold = 0.1f;

		[Export] public double HoldTime = 0.0f;

		public override void _Ready()
		{
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{

		}
	}
}