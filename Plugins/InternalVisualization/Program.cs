using ETS2LA.Logging;
using ETS2LA.Game;
using ETS2LA.AR;
using ETS2LA.Shared;
using ETS2LA.Backend.Events;

using InternalVisualization.Renderers;

using TruckLib.ScsMap;
using Hexa.NET.ImGui;
using System.Numerics;

namespace InternalVisualization
{
    public class InternalVisualization : Plugin
    {
        public override PluginInformation Info => new PluginInformation
        {
            Name = "InternalVisualization",
            Description = "Consectetur mollit ipsum velit Lorem fugiat aliqua officia exercitation exercitation.",
            AuthorName = "Developer",
        };

        private WindowDefinition _windowDefinition = new WindowDefinition
        {
            Title = "Internal Visualization",
            Width = 800,
            Height = 600,
        };

        public override float TickRate => 1f;

        private Renderer[] renderers = new Renderer[]
        {
            new TruckRenderer(),
            new NodesRenderer()
        };

        private GameTelemetryData? _telemetryData;
        private MapData? _mapData;
        private Road[]? _roads;
        private Prefab[]? _prefabs;

        public override void Init()
        {
            base.Init();
            // This is run once when the plugin is initially loaded.
            // Usually you start to listen to control events here (or register your own).
            // ControlHandler.Current.On(ControlHandler.Defaults.Next.Id, OnNextPressed);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            ARHandler.Current.RegisterWindow(_windowDefinition, RenderWindow);

            // Subscribe to events here, do not subscribe in Init as that's too early.
            // Events.Current.Subscribe<YourEventType>("YourTopic", YourEventHandler);
            Events.Current.Subscribe<GameTelemetryData>("GameTelemetry.Data", OnTelemetryUpdated);
        }

        private void OnTelemetryUpdated(GameTelemetryData data)
        {
            _telemetryData = data;
        }

        public override void Tick()
        {
            int installation = 0;
            bool found = false;
            foreach (var item in GameHandler.Current.Installations)
            {
                if(item.IsParsed) {
                    found = true; 
                    break;
                }
                installation++;
            }

            if (!found) return;
            _mapData = GameHandler.Current.Installations[installation].GetMapData();
            _roads = _mapData.MapItems.Where(item => item is Road).Cast<Road>().ToArray();
            _prefabs = _mapData.MapItems.Where(item => item is Prefab).Cast<Prefab>().ToArray();
        }

        private void RenderWindow()
        {
            if(_mapData == null)
            {
                ImGui.Text("Waiting for map data...");
                return;
            }

            var drawList = ImGui.GetWindowDrawList();
            var windowSize = ImGui.GetWindowSize();
            var windowPos = ImGui.GetWindowPos();

            foreach (var renderer in renderers)
            {
                renderer.Render(drawList, windowPos, windowSize, _telemetryData!, _mapData!, _roads!, _prefabs!);
            }
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            _mapData = null;
            ARHandler.Current.UnregisterWindow(_windowDefinition);
            // Unsubscribe from events here
            // Events.Current.Unsubscribe<YourEventType>("YourTopic", YourEventHandler);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            // This is run once when the plugin is unloaded (at app shutdown), use it to clean up any resources or
            // threads you created in Init or elsewhere.
        }
    }
}