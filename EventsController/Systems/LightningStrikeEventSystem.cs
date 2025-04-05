using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.Prefabs;
using Unity.Entities;

namespace EventsController.Systems
{
    public partial class LightningStrikeEventSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        
        public void LightningStrikeOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                WPD.m_OccurenceProbability = 0;
                EntityManager.SetComponentData(prefabEntity, WPD);
            }            
        }
        public void ResetLightningStrikeOccurence(PrefabID prefabID)
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
        public void ControlLightningStrikes(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
            && prefab.TryGet(out Game.Prefabs.WeatherPhenomenon WP)
            && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
            && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.WeatherPhenomenonData WPD))
            {
                WPD.m_LightningInterval = new Bounds1(Mod.m_Setting.LightningIntervalMin, Mod.m_Setting.LightningIntervalMax);
                WPD.m_Duration = new Bounds1(Mod.m_Setting.DurationMin, Mod.m_Setting.DurationMax);
                WPD.m_OccurenceTemperature = new Bounds1(Mod.m_Setting.OccurenceTemperatureMin, Mod.m_Setting.OccurenceTemperatureMax);
                EntityManager.SetComponentData(prefabEntity, WPD);
            }
        }
        public void ControlLightningStrikeFires(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && prefab.TryGet(out Game.Prefabs.Fire fire)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                float fireStartProbability = fire.m_StartProbability;
                float fireStartIntensity = fire.m_StartIntensity;
                float fireEscalationRate = fire.m_EscalationRate;
                float fireSpreadProbability = fire.m_SpreadProbability;
                float fireSpreadRange = fire.m_SpreadRange;
                
                
                fireData.m_StartProbability = fireStartProbability * (Mod.m_Setting.LightningFireStartProbability / 100.0f);
                fireData.m_StartIntensity = fireStartIntensity * (Mod.m_Setting.LightningFireStartIntensity / 100.0f);
                fireData.m_EscalationRate = fireEscalationRate * (Mod.m_Setting.LightningFireEscalationRate / 100.0f);
                fireData.m_SpreadProbability = fireSpreadProbability * (Mod.m_Setting.LightningFireSpreadProbability / 100.0f);
                fireData.m_SpreadRange = fireSpreadRange * (Mod.m_Setting.LightningFireSpreadRange / 100.0f);
                EntityManager.SetComponentData(prefabEntity, fireData);
            }
        }
        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
        }
        protected override void OnUpdate()
        { 
            HandleLightningControls(true, EventPrefabs.LightningStrikePrefabID);
            HandleLightningOccurence(Mod.m_Setting.LightningStrikeOccurenceToggle, EventPrefabs.LightningStrikePrefabID);
            ControlLightningStrikes(EventPrefabs.LightningStrikePrefabID);
            ControlLightningStrikeFires(EventPrefabs.LightningStrikePrefabID);
        }
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleLightningOccurence(Mod.m_Setting.LightningStrikeOccurenceToggle, EventPrefabs.LightningStrikePrefabID);
            HandleLightningControls(true, EventPrefabs.LightningStrikePrefabID);
            
        }
        private void HandleLightningOccurence(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                LightningStrikeOccurenceToZero(prefabID);
            }
            else
            {
                ResetLightningStrikeOccurence(prefabID);
            }
        }
        private void HandleLightningControls(bool apply, PrefabID prefabID)
        {
            if (apply)
            {
                ControlLightningStrikes(prefabID);
                ControlLightningStrikeFires(prefabID);
            }
        }
    }
}