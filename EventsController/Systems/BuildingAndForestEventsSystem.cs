using Colossal.Entities;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;

namespace EventsController.Systems
{
    public partial class BuildingAndForestEventsSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        private CityConfigurationSystem m_CityConfigurationSystem;
        private ClimateSystem m_ClimateSystem; // Reference to ClimateSystem
        
        // Summer season constants - adjust these as needed
        private const float SUMMER_FIRE_MULTIPLIER = 2.0f; // Double forest fire probability in summer
        private readonly string[] SUMMER_SEASON_IDS = { "season_summer", "summer" }; // Possible summer season IDs
        
        public void BuildingCollapseOccurenceToZero(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.DestructionData data))
            {
                data.m_OccurenceProbability = 0;
                EntityManager.SetComponentData(prefabEntity, data);
            }            
        }
        
        public void ResetBuildingCollapseOccurences(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab) 
                && prefab.TryGet(out Game.Prefabs.Destruction destruction)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.DestructionData data))
            {
                data.m_OccurenceProbability = destruction.m_OccurenceProbability;
                EntityManager.SetComponentData(prefabEntity, data);
            }
        }
        
        public void ControlBuildingFires(PrefabID prefabID)
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
                
                fireData.m_StartProbability = fireStartProbability * (Mod.m_Setting.BuildingFireStartProbability / 100.0f);
                fireData.m_StartIntensity = fireStartIntensity * (Mod.m_Setting.BuildingFireStartIntensity / 100.0f);
                fireData.m_EscalationRate = fireEscalationRate * (Mod.m_Setting.BuildingFireEscalationRate / 100.0f);
                fireData.m_SpreadProbability = fireSpreadProbability * (Mod.m_Setting.BuildingFireSpreadProbability / 100.0f);
                fireData.m_SpreadRange = fireSpreadRange * (Mod.m_Setting.BuildingFireSpreadRange / 100.0f);
                EntityManager.SetComponentData(prefabEntity, fireData);
            }
        }
        
        public void ControlForestFires(PrefabID prefabID)
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
                
                
                float seasonMultiplier = ShouldApplySummerFireMultiplier() ? SUMMER_FIRE_MULTIPLIER : 1.0f;
                
               
                fireData.m_StartProbability = Mod.m_Setting.ForestFireStartProbability  * seasonMultiplier;
                fireData.m_StartIntensity = Mod.m_Setting.ForestFireStartIntensity;
                fireData.m_EscalationRate = Mod.m_Setting.ForestFireEscalationRate;
                fireData.m_SpreadProbability = Mod.m_Setting.ForestFireSpreadProbability  * seasonMultiplier;
                fireData.m_SpreadRange = Mod.m_Setting.ForestFireSpreadRange;
                
                EntityManager.SetComponentData(prefabEntity, fireData);
            }
        }
        
        // Check if we should apply the summer fire multiplier - needs both setting enabled and current season to be summer
        private bool ShouldApplySummerFireMultiplier()
        {
            return Mod.m_Setting.EnableSummerFireIncrease && IsSummerSeason();
        }
        
        // Check if current season is summer
        private bool IsSummerSeason()
        {
            if (m_ClimateSystem == null || string.IsNullOrEmpty(m_ClimateSystem.currentSeasonNameID))
                return false;
            
            string currentSeason = m_ClimateSystem.currentSeasonNameID.ToLower();
            
            foreach (string summerID in SUMMER_SEASON_IDS)
            {
                if (currentSeason.Contains(summerID))
                    return true;
            }
            
            return false;
        }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            m_CityConfigurationSystem = World.GetOrCreateSystemManaged<CityConfigurationSystem>();
            m_ClimateSystem = World.GetOrCreateSystemManaged<ClimateSystem>(); // Get ClimateSystem
        }
        
        protected override void OnUpdate()
        { 
            // Update forest fire settings every frame to account for season changes
            HandleBFandFFControls(true, EventPrefabs.LightningStrikePrefabID);
            HandleBFandFFControls(true, EventPrefabs.ForestFirePrefabID);
        }
        
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleBuildingCollapseOccurence(Mod.m_Setting.BuildingCollapseOccurenceToggle, EventPrefabs.BuildingCollapseID);
            HandleBFandFFControls(true, EventPrefabs.BuildingFirePrefabID);
            HandleBFandFFControls(true, EventPrefabs.ForestFirePrefabID);
        }
        
        private void HandleBuildingCollapseOccurence(bool toggle, PrefabID prefabID)
        {
            if (toggle)
            {
                BuildingCollapseOccurenceToZero(prefabID);
            }
            else
            {
                ResetBuildingCollapseOccurences(prefabID);
            }
        }
        
        private void HandleBFandFFControls(bool apply, PrefabID prefabID)
        {
            if (apply)
            {
                ControlBuildingFires(prefabID);
                ControlForestFires(prefabID);
            }
        }
    }
}