using Godot;
using System;

namespace PokeEmerald.Characters;

public partial class CharacterCollisonRayCast : RayCast2D
{
	[Signal]
	public delegate void CollisionEventHandler(bool collided);

	[ExportCategory("Collision Vars")] 

	[Export] public GodotObject Collider;
	private bool _disableCollision = false;

	public void CheckCollision()
	{
		if (_disableCollision) return;
		if (IsColliding())
		{
			Collider = GetCollider();
			EmitSignal(SignalName.Collision, true);
		}
		else
		{
			EmitSignal(SignalName.Collision, false);
		}
	}
	
	public override void _Process(double delta)
	{
		if (_disableCollision) return;
		CheckCollision();
	}

	public void DisableCollision()
	{
		_disableCollision = true;
		EmitSignal(SignalName.Collision, false);
	}

	public void EnableCollision()
	{
		_disableCollision = false;
		EmitSignal(SignalName.Collision, false);
	}
}
