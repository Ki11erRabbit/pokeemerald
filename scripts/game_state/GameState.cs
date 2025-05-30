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
	[Export] public BikeState BikeState { get; set; } = BikeState.AcroBike;
	[Export] public bool RidingBike { get; private set; } = false;
	
	/// <summary>
	/// Member <c>DoingWheelie</c> is transient and shouldn't be saved when the game saves.
	/// </summary>
	[Export] public bool DoingWheelie { get; private set; } = false;
	/// <summary>
	/// Member <c>DoingWheelieBounce</c> is transient and shouldn't be saved when the game saves.
	/// </summary>
	[Export] public bool DoingWheelieBounce { get; private set; } = false;

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

	public static void DoWheelie()
	{
		Debug.Assert(CanRideBike(), "Player shouldn't be able to ride bike");
		Instance.DoingWheelie = true;
	}

	public static void StopDoingWheelie()
	{
		Instance.DoingWheelie = false;
		Instance.DoingWheelieBounce = false;
	}

	public static void DoWheelieBounce()
	{
		Debug.Assert(CanRideBike(), "Player shouldn't be able to ride bike");
		Debug.Assert(Instance.DoingWheelie, "Player should be in doing wheelie state");
		Instance.DoingWheelieBounce = true;
	}

	public static void StopDoingWheelieBounce()
	{
		Instance.DoingWheelieBounce = false;
		Instance.DoingWheelie = false;
	}
	
	public override void _Ready()
	{
		Instance = this;
	}

}
