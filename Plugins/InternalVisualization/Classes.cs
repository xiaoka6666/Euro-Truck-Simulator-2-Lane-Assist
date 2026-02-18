using ETS2LA.Game;
using ETS2LA.Shared;
using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;

namespace InternalVisualization;

public class Renderer
{
    public int ViewDistance = 300;
    public virtual void Render(ImDrawListPtr drawList, Vector2 windowPos, Vector2 windowSize, 
                               GameTelemetryData telemetryData, MapData mapData, Road[] roads, Prefab[] prefabs)
    {
        // Implement your rendering logic here using the provided data.
        // This is where you would use ImGui to visualize the data.
    }
}