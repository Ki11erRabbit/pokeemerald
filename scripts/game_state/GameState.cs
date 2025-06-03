using Godot;
using System;

namespace PokeEmerald.GameState;

public enum BikeState
{
	NoBike,
	AcroBike,
	MachBike,
}

public partial class GameState : Node
{
	public static GameState Instance { get; private set; }
	
	[ExportCategory("Flags")]
	[Export] public BikeState BikeState { get; set; } = BikeState.MachBike;
	[Export] public bool RidingBike { get; private set; } = false;
	
	public static bool CanRideBike()
	{
		return Instance.BikeState != BikeState.NoBike;
	}

	public static bool RidingAcroBike()
	{
		return Instance.RidingBike && Instance.BikeState switch
		{
			BikeState.AcroBike => true,
			_ => false,
		};
	}

	public static void RideBike()
	{
		if (CanRideBike())
		{
			Instance.RidingBike = true;
		}
	}

	public static void StopRidingBike()
	{
		Instance.RidingBike = false;
	}
	
	public override void _Ready()
	{
		Instance = this;
	}

}
