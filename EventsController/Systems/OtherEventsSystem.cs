using Colossal.Entities;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.Prefabs;
using Unity.Entities;

namespace EventsController.Systems
{
    public partial  class OtherEventsSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        
        public void RobberyController(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.CrimeData crimeData))
            {
                crimeData.m_OccurenceProbability = new Bounds1(Mod.m_Setting.OccurenceProbabilityMin, Mod.m_Setting.OccurenceProbabilityMax);
                crimeData.m_RecurrenceProbability = new Bounds1(Mod.m_Setting.RecurrenceProbabilityMin, Mod.m_Setting.RecurrenceProbabilityMax);
                EntityManager.SetComponentData(prefabEntity, crimeData);
            }            
        }
        public void RobberyOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.CrimeData crimeData))
            {
                crimeData.m_OccurenceProbability = new Bounds1(0, 0);
                crimeData.m_RecurrenceProbability = new Bounds1(0, 0);
                EntityManager.SetComponentData(prefabEntity, crimeData);
            }
        }
        public void ResetRobberyOccurence(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.Crime crime)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.CrimeData crimeData))
            {
                
                
                crimeData.m_OccurenceProbability = new Bounds1(crime.m_OccurenceProbability.min, crime.m_OccurenceProbability.max);
                crimeData.m_RecurrenceProbability = new Bounds1(crime.m_RecurrenceProbability.min, crime.m_RecurrenceProbability.max);
                EntityManager.SetComponentData(prefabEntity, crimeData);
            }
        }
        
        
        public void LCAFirestartProbabilityToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                fireData.m_StartProbability = 0;
                EntityManager.SetComponentData(prefabEntity, fireData);
            }            
        }
        public void ResetLCAFireStartProbability(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.Fire fire)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                fireData.m_StartProbability = fire.m_StartProbability;
                EntityManager.SetComponentData(prefabEntity, fireData);
            }
        }
        public void LCAAccidentOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.TrafficAccidentData trafficAccidentData))
            {
                trafficAccidentData.m_OccurenceProbability = 0;
                EntityManager.SetComponentData(prefabEntity, trafficAccidentData);
            }            
        }
        public void ResetLCAAccidentOccurence(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.TrafficAccident trafficAccident)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.TrafficAccidentData trafficAccidentData))
            {
                trafficAccidentData.m_OccurenceProbability = trafficAccident.m_OccurrenceProbability;
                EntityManager.SetComponentData(prefabEntity, trafficAccidentData);
            }
        }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
        }
        protected override void OnUpdate()
        {
            HandleRobberyOccurences(true, EventPrefabs.RobberyID);
            HandleLCAFireStartProbability(Mod.m_Setting.LCAFireStartProbabilityToggle, EventPrefabs.LoseControlAccidentPrefabID);
            HandleLCAAccidentOccurences(Mod.m_Setting.LCAAccidentOccurenceToggle, EventPrefabs.LoseControlAccidentPrefabID);
        }
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleRobberyOccurences(Mod.m_Setting.RobberyOccurenceToggle, EventPrefabs.RobberyID);
            HandleLCAFireStartProbability(Mod.m_Setting.LCAFireStartProbabilityToggle, EventPrefabs.LoseControlAccidentPrefabID);
            HandleLCAAccidentOccurences(Mod.m_Setting.LCAAccidentOccurenceToggle, EventPrefabs.LoseControlAccidentPrefabID);
            
        }
        
        private void HandleRobberyOccurences(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                RobberyOccurenceToZero(prefabID);
            }
            else
            {
                ResetRobberyOccurence(prefabID);
                RobberyController(prefabID);
            }
        }
        private void HandleLCAFireStartProbability(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                LCAFirestartProbabilityToZero(prefabID);
            }
            else
            {
                ResetLCAFireStartProbability(prefabID);
            }
        }
        private void HandleLCAAccidentOccurences(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                LCAAccidentOccurenceToZero(prefabID);
            }
            else
            {
                ResetLCAAccidentOccurence(prefabID);
            }
        }    
    }
}