using Godot;
using System;

namespace PokeEmerald;
public partial class Globals : Node
{
    [ExportCategory("Constants")] 
    [Export] public long TileSize { get; set; } = 16;
    [Export] public double WalkingSpeed { get; set; } = 4.0;
    [Export] public double RunningSpeed { get; set; } = 8.0;
    [Export] public double AcroCyclingSpeed { get; set; } = 10.67;
    [Export] public double AcroCyclingWheelieSpeed { get; set; } = 8.0;
    [Export] public double MachCyclingSpeed { get; set; } = 16;
    [Export] public double SwimmingSpeed { get; set; } = 8.0;
    [Export] public double DivingSpeed { get; set; } = 5.0;
    
    public static Globals Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public Vector2 SnapToGrid(Vector2 pos)
    {
        return new Vector2(
            Mathf.Round(pos.X / TileSize) * TileSize,
            Mathf.Round(pos.Y / TileSize) * TileSize
        );
    }

    public double GetBikeSpeed()
    {
        Debug.Assert(GameState.GameState.CanRideBike(), "Player shouldn't be able to ride bike");
        return GameState.GameState.Instance.BikeState switch
        {
            GameState.BikeState.NoBike => 0.0,
            GameState.BikeState.AcroBike => GameState.GameState.Instance.DoingWheelie ? AcroCyclingWheelieSpeed : AcroCyclingSpeed,
            GameState.BikeState.MachBike => MachCyclingSpeed,
        };
    }
}
