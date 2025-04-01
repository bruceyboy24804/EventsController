using Game.Prefabs;

namespace EventsController.Domain
{
    public class EventPrefabs
    {
        public static readonly  PrefabID LightningStrikePrefabID = new PrefabID("EventPrefab", "Lightning Strike");
        public static readonly PrefabID TornadoPrefabID = new PrefabID("EventPrefab", "Tornado");
        public static readonly PrefabID BuildingCollapseID = new PrefabID("EventPrefab", "Building Collapse");
        public static readonly PrefabID RobberyID = new PrefabID("EventPrefab", "Robbery");
        public static readonly PrefabID HailStormID = new PrefabID("EventPrefab", "Hail Storm");
        public static readonly PrefabID LoseControlAccidentPrefabID = new PrefabID("EventPrefab", "Lose Control Accident");
        public static readonly PrefabID ForestFirePrefabID = new PrefabID("EventPrefab", "Forest Fire");
        public static readonly PrefabID BuildingFirePrefabID = new PrefabID("EventPrefab", "Building Fire");
    }
}