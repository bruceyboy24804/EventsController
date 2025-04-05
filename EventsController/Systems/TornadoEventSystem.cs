using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.City;
using Game.Prefabs;
using Unity.Entities;

namespace EventsController.Systems
{
    public partial class TornadoEventSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        public void TornadoOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                WPD.m_OccurenceProbability = 0;
                EntityManager.SetComponentData(prefabEntity, WPD);
            }            
        }
        public void ResetTornadoOccurence(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.WeatherPhenomenon wP)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData wPD))
            {
                wPD.m_OccurenceProbability = wP.m_OccurrenceProbability;
                EntityManager.SetComponentData(prefabEntity, wPD);
            }
        }
        public void ControlTornadoes(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
            && prefab.TryGet(out Game.Prefabs.WeatherPhenomenon WP)
            && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
            && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                WPD.m_DamageSeverity = Mod.m_Setting.TornadoDamageSeverity;
                WPD.m_Duration = new Bounds1(Mod.m_Setting.TornadoDurationMin, Mod.m_Setting.TornadoDurationMax);
                WPD.m_OccurenceTemperature = new Bounds1(Mod.m_Setting.TornadoOccurenceTemperatureMin, Mod.m_Setting.TornadoOccurenceTemperatureMax);
                WPD.m_OccurenceRain = new Bounds1(Mod.m_Setting.TornadoOccurenceRainMin, Mod.m_Setting.TornadoOccurenceRainMax);
                EntityManager.SetComponentData(prefabEntity, WPD);
            }
        }
        public void ControlTornadoTrafficAccidents(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && prefab.TryGet(out Game.Prefabs.TrafficAccident tD)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.TrafficAccidentData tAD))
            {
                tAD.m_OccurenceProbability = Mod.m_Setting.TornadoTrafficAccidentOccurenceProbability;
                EntityManager.SetComponentData(prefabEntity, tAD);
            }
        }
        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
        }
        protected override void OnUpdate()
        {
            HandleTornadoControls(true, EventPrefabs.TornadoPrefabID);
            HandleTornadoOccurence(Mod.m_Setting.TornadoOccurenceToggle, EventPrefabs.TornadoPrefabID);
        }
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleTornadoControls(true, EventPrefabs.TornadoPrefabID);
            HandleTornadoOccurence(Mod.m_Setting.TornadoOccurenceToggle, EventPrefabs.TornadoPrefabID);
        }
        private void HandleTornadoOccurence(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                TornadoOccurenceToZero(prefabID);
            }
            else
            {
                ResetTornadoOccurence(prefabID);
            }
        }
        private void HandleTornadoControls(bool apply, PrefabID prefabID)
        {
            if (apply)
            {
                ControlTornadoes(prefabID);
                ControlTornadoTrafficAccidents(prefabID);
            }
        }   
    }
}