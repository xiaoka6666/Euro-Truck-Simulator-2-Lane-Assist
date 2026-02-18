using ETS2LA.Game;
using ETS2LA.Shared;
using ETS2LA.Backend.Events;
using ETS2LA.Logging;

using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;
using TruckLib;

namespace InternalVisualization.Renderers;

public class TrafficRenderer : Renderer
{
    private TrafficData? _trafficData;
    public TrafficRenderer()
    {
        Events.Current.Subscribe<TrafficData>("ETS2LASDK.Traffic", OnTrafficDataReceived);
    }

    private void OnTrafficDataReceived(TrafficData data)
    {
        _trafficData = data;
    }

    private void DrawRectangle(ImDrawListPtr drawList, Vector2 screenPos, float width, float length, float angle)
    {
        width *= InternalVisualizationConstants.Scale;
        length *= InternalVisualizationConstants.Scale;

        Vector2[] corners = new Vector2[]
        {
            new Vector2(-length / 2, -width / 2),
            new Vector2(length / 2, -width / 2),
            new Vector2(length / 2, width / 2),
            new Vector2(-length / 2, width / 2)
        };

        // Rotate the truck corners around the center point.
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = Vector2.Transform(
                corners[i], Matrix4x4.CreateRotationZ(Utils.ToRadians(angle))
                
            ) + screenPos;
        }

        for (int i = 0; i < corners.Length; i++)
        {
            Vector2 start = corners[i];
            Vector2 end = corners[(i + 1) % corners.Length];
            drawList.AddLine(start, end, ImGui.GetColorU32(new Vector4(0.6f, 0.6f, 0.6f, 1)), 2 * InternalVisualizationConstants.Scale);
        }
    }

    public override void Render(ImDrawListPtr drawList, Vector2 windowPos, Vector2 windowSize, 
                                GameTelemetryData telemetryData, MapData mapData, Road[] roads, Prefab[] prefabs, IReadOnlyList<Node> nearbyNodes)
    {
        foreach (var vehicle in _trafficData?.vehicles ?? Array.Empty<TrafficVehicle>())
        {
            Vector3 center = vehicle.position;
            Vector2 screenPos = Utils.WorldToScreen(center, telemetryData.truckPlacement.coordinate.ToVector3(), windowSize) + windowPos;

            ETS2LA.Shared.Quaternion rotation = vehicle.rotation;
            float angle = new System.Numerics.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W).ToEulerDeg().Y + 90f;

            float width = vehicle.size.X;
            float length = vehicle.size.Z;

            DrawRectangle(drawList, screenPos, width, length, angle);

            foreach (var trailer in vehicle.trailers)
            {
                Vector3 trailerCenter = trailer.position;
                Vector2 trailerScreenPos = Utils.WorldToScreen(trailerCenter, telemetryData.truckPlacement.coordinate.ToVector3(), windowSize) + windowPos;

                ETS2LA.Shared.Quaternion trailerRotation = trailer.rotation;
                float trailerAngle = new System.Numerics.Quaternion(trailerRotation.X, trailerRotation.Y, trailerRotation.Z, trailerRotation.W).ToEulerDeg().Y + 90f;

                float trailerWidth = trailer.size.X;
                float trailerLength = trailer.size.Z;

                DrawRectangle(drawList, trailerScreenPos, trailerWidth, trailerLength, trailerAngle);
            }
        }
    }
}