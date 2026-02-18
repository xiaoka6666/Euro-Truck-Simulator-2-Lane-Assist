using ETS2LA.Game;
using ETS2LA.Shared;
using ETS2LA.Logging;

using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;
using TruckLib;

namespace InternalVisualization.Renderers;

public class NodesRenderer : Renderer
{
    public override void Render(ImDrawListPtr drawList, Vector2 windowPos, Vector2 windowSize, 
                                GameTelemetryData telemetryData, MapData mapData, Road[] roads, Prefab[] prefabs, IReadOnlyList<Node> nearbyNodes)
    {
        Vector3Double center = telemetryData.truckPlacement.coordinate;
        foreach (var node in nearbyNodes)
        {
            Vector2 screenPos = Utils.WorldToScreen(node.Position, center.ToVector3(), windowSize);
            drawList.AddCircle(screenPos + windowPos, 3 * InternalVisualizationConstants.Scale, ImGui.GetColorU32(new Vector4(1, 1, 1, 0.1f)));
        }
    }
}