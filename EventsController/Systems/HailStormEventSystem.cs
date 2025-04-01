using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.Prefabs;
using Unity.Entities;

namespace EventsController.Systems
{
    public partial class HailStormEventSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        
        public void HSOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                WPD.m_OccurenceProbability = 0;
                EntityManager.SetComponentData(prefabEntity, WPD);
            }            
        }
        public void ResetHSOccurence(PrefabID prefabID)
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
        public void ControlHS(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
            && prefab.TryGet(out Game.Prefabs.WeatherPhenomenon WP)
            && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
            && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                
                WPD.m_DamageSeverity = Mod.m_Setting.HSDamageSeverity;
                WPD.m_Duration = new Bounds1(Mod.m_Setting.HSDurationMin, Mod.m_Setting.HSDurationMax);
                WPD.m_OccurenceTemperature = new Bounds1(Mod.m_Setting.HSTemperatureMin, Mod.m_Setting.HSTemperatureMax);
                WPD.m_OccurenceRain = new Bounds1(Mod.m_Setting.HSRainMin, Mod.m_Setting.HSRainMax);
                EntityManager.SetComponentData(prefabEntity, WPD);
            }
        }
        public void ControlHSTrafficAccidents(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && prefab.TryGet(out Game.Prefabs.TrafficAccident tD)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.TrafficAccidentData tAD))
            {
                tAD.m_OccurenceProbability = Mod.m_Setting.HSTrafficAccidentOccurenceProbability;
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
            HandleHSControls(true, EventPrefabs.HailStormID);
        }
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleHSControls(true, EventPrefabs.HailStormID);
            HandleHSOccurence(Mod.m_Setting.HsOccurenceToggle, EventPrefabs.HailStormID);
            
        }
        private void HandleHSOccurence(bool toggle, PrefabID prefabID)
        {
            if (toggle)
            {
                HSOccurenceToZero(prefabID);
            }
            else
            {
                ResetHSOccurence(prefabID);
            }
        }
        private void HandleHSControls(bool apply, PrefabID prefabID)
        {
            if (apply)
            {
                ControlHS(prefabID);
                ControlHSTrafficAccidents(prefabID);
            }
        }
    }
}