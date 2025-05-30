using System;
using Godot;

namespace PokeEmerald;

internal static class Debug
{
    internal static void Assert(bool cond, string msg)
#if DEBUG
    {
        if (cond) return;
        
        GD.PrintErr(msg);
        throw new ApplicationException($"Assert failed: {msg}");
    }
    #else
    {
        GD.PrintErr(msg);
    }
#endif
    
    internal static void Log(string msg)
#if DEBUG
    {
        GD.Print(msg);
    }
#else
    {
        
    }
#endif
}