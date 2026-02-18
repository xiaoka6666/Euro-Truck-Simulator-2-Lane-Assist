using ETS2LA.Game;
using ETS2LA.Shared;
using ETS2LA.Logging;

using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;
using TruckLib;
using ETS2LA.Game.Utils;

namespace InternalVisualization.Renderers;

public class RoadsRenderer : Renderer
{
    private List<string> invalidRoadTypes = new List<string>();
    private Dictionary<string, (float[] Left, float[] Right)> roadLaneCache = new Dictionary<string, (float[] Left, float[] Right)>();
    
    public override void Render(ImDrawListPtr drawList, Vector2 windowPos, Vector2 windowSize, 
                                GameTelemetryData telemetryData, MapData mapData, Road[] roads, Prefab[] prefabs, IReadOnlyList<Node> nearbyNodes)
    {
        Vector3Double center = telemetryData.truckPlacement.coordinate;
        Dictionary<ulong, Road> nearbyRoads = new Dictionary<ulong, Road>();

        foreach (var node in nearbyNodes)
        {
            if(node.BackwardItem is Road)
            {
                Road road = (Road)node.BackwardItem;
                if (!nearbyRoads.ContainsKey(road.Uid))
                {
                    nearbyRoads.Add(road.Uid, road);
                }
            }

            if(node.ForwardItem is Road)
            {
                Road road = (Road)node.ForwardItem;
                if (!nearbyRoads.ContainsKey(road.Uid))
                {
                    nearbyRoads.Add(road.Uid, road);
                }
            }
        }

        foreach (var road in nearbyRoads.Values)
        {
            if (invalidRoadTypes.Contains(road.RoadType.ToString())) continue;
            float resolution = 5f;
            float length = road.Length;

            float[] steps = new float[(int)(length / resolution) + 1];
            for (int i = 0; i < steps.Length; i++)
            {
                steps[i] = i * resolution;
            }

            List<OrientedPoint> roadPoints = new List<OrientedPoint>();
            for (int i = 0; i < steps.Length; i++)
            {
                bool isLast = false;
                if (i == steps.Length - 1) isLast = true;

                OrientedPoint? point = isLast ? road.InterpolateCurve(1) : road.InterpolateCurveDist(steps[i]);
                if (point == null) continue;
                
                roadPoints.Add(point.Value);
            }

            if (road.RoadType.ToString() == "") continue;
            float[] left, right;
            if (roadLaneCache.ContainsKey(road.RoadType.ToString()))
            {
                (left, right) = roadLaneCache[road.RoadType.ToString()];
            }
            else
            {
                (left, right) = RoadUtils.CalculateRoadLaneCenters(road);
                roadLaneCache.Add(road.RoadType.ToString(), (left, right));
            }

            if (left.Length == 0 && right.Length == 0)
            {
                if (!invalidRoadTypes.Contains(road.RoadType.ToString()))
                {
                    invalidRoadTypes.Add(road.RoadType.ToString());
                    Logger.Warn($"Road type {road.RoadType} has no lane data. It will not be rendered.");
                }
                continue;
            }

            foreach (var laneOffset in left)
            {
                List<Vector3> lanePoints = new List<Vector3>();
                foreach (var roadPoint in roadPoints)
                {
                    Vector3 normal = Vector3.Normalize(Vector3.Transform(Vector3.UnitX, roadPoint.Rotation));
                    Vector3 pointOnLane = roadPoint.Position + normal * -laneOffset;
                    lanePoints.Add(pointOnLane);
                }

                for (int i = 0; i < lanePoints.Count - 1; i++)
                {
                    Vector2 start = Utils.WorldToScreen(lanePoints[i], center.ToVector3(), windowSize) + windowPos;
                    Vector2 end = Utils.WorldToScreen(lanePoints[i + 1], center.ToVector3(), windowSize) + windowPos;
                    drawList.AddLine(start, end, ImGui.GetColorU32(new Vector4(0.6f, 0.4f, 0.4f, 1)), 2 * InternalVisualizationConstants.Scale);
                }
            }

            foreach (var laneOffset in right)
            {
                List<Vector3> lanePoints = new List<Vector3>();
                foreach (var roadPoint in roadPoints)
                {
                    Vector3 normal = Vector3.Normalize(Vector3.Transform(Vector3.UnitX, roadPoint.Rotation));
                    Vector3 pointOnLane = roadPoint.Position + normal * -laneOffset;
                    lanePoints.Add(pointOnLane);
                }

                for (int i = 0; i < lanePoints.Count - 1; i++)
                {
                    Vector2 start = Utils.WorldToScreen(lanePoints[i], center.ToVector3(), windowSize) + windowPos;
                    Vector2 end = Utils.WorldToScreen(lanePoints[i + 1], center.ToVector3(), windowSize) + windowPos;
                    drawList.AddLine(start, end, ImGui.GetColorU32(new Vector4(0.4f, 0.6f, 0.4f, 1)), 2 * InternalVisualizationConstants.Scale);
                }
            }
        }
    }
}