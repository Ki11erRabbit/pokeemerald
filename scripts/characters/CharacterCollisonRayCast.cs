using Godot;
using System;

namespace PokeEmerald.Characters;

public partial class CharacterCollisonRayCast : RayCast2D
{
	[Signal]
	public delegate void CollisionEventHandler(bool collided, GodotObject collidedObject);

	[ExportCategory("Collision Vars")] 
	[Export] public CharacterController CharacterController;

	[Export] public GodotObject Collider;

	public void CheckCollision()
	{
		if (IsColliding())
		{
			Collider = GetCollider();

			EmitSignal(SignalName.Collision, true, Collider);
		}
		else
		{
			EmitSignal(SignalName.Collision, false, new Variant());
		}
	}
	
	public override void _Process(double delta)
	{
		CheckCollision();
	}
}
