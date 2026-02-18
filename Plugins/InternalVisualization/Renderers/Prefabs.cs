using ETS2LA.Game;
using ETS2LA.Shared;
using ETS2LA.Logging;

using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;
using TruckLib;
using Avalonia.Controls;

namespace InternalVisualization.Renderers;

public class PrefabsRenderer : Renderer
{
    public override void Render(ImDrawListPtr drawList, Vector2 windowPos, Vector2 windowSize, 
                                GameTelemetryData telemetryData, MapData mapData, Road[] roads, Prefab[] prefabs, IReadOnlyList<Node> nearbyNodes)
    {
        Vector3Double center = telemetryData.truckPlacement.coordinate;
        Dictionary<ulong, Prefab> nearbyPrefabs = new Dictionary<ulong, Prefab>();

        foreach (var node in nearbyNodes)
        {
            if(node.BackwardItem is Prefab)
            {
                Prefab prefab = (Prefab)node.BackwardItem;
                if (!nearbyPrefabs.ContainsKey(prefab.Uid))
                {
                    nearbyPrefabs.Add(prefab.Uid, prefab);
                }
            }

            if(node.ForwardItem is Prefab)
            {
                Prefab prefab = (Prefab)node.ForwardItem;
                if (!nearbyPrefabs.ContainsKey(prefab.Uid))
                {
                    nearbyPrefabs.Add(prefab.Uid, prefab);
                }
            }
        }

        //foreach (var prefab in nearbyPrefabs.Values)
        //{
        //    float resolution = 1f;
        //    float length = road.Length;

        //    float[] steps = new float[(int)(length / resolution) + 1];
        //    for (int i = 0; i < steps.Length; i++)
        //    {
        //        steps[i] = i * resolution;
        //    }

        //    for(int i = 0; i < steps.Length - 1; i++)
        //    {
        //        bool isEnd = false;
        //        if (steps[i + 1] > length) isEnd = true;

        //        OrientedPoint? startPoint = road.InterpolateCurveDist(steps[i]);
        //        if (startPoint == null) continue;
        //        Vector2 start = Utils.WorldToScreen(startPoint.Value.Position, center.ToVector3(), windowSize) + windowPos;

        //        OrientedPoint? endPoint = !isEnd ? road.InterpolateCurveDist(steps[i + 1]) : road.InterpolateCurve(1);
        //        if (endPoint == null) continue;
        //        Vector2 end = Utils.WorldToScreen(endPoint.Value.Position, center.ToVector3(), windowSize) + windowPos;

        //        drawList.AddLine(start, end, ImGui.GetColorU32(new Vector4(0.6f, 0.6f, 0.6f, 1)), 2);
        //    }
        //}
    }
}