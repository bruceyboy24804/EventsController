using Colossal.Entities;
using Colossal.Serialization.Entities;
using EventsController.Domain;
using Game;
using Game.City;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Mathematics;

namespace EventsController.Systems
{
    public partial class BuildingAndForestEventsSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;
        private CityConfigurationSystem m_CityConfigurationSystem;
        private ClimateSystem m_ClimateSystem; // Reference to ClimateSystem
        private LightningStrikeEventSystem m_LightningStrikeSystem; // Reference to LightningStrikeEventSystem
        
        // Summer season constants - adjust these as needed
        private const float SUMMER_FIRE_MULTIPLIER = 2.0f; // Double forest fire probability in summer
        private readonly string[] SUMMER_SEASON_IDS = { "season_summer", "summer" }; // Possible summer season IDs
        
        // Weather tracking for forest fire calculations
        private const int HISTORY_SIZE = 30; // Days of weather history to track
        private Queue<WeatherRecord> m_WeatherHistory = new Queue<WeatherRecord>(HISTORY_SIZE);
        private float m_CurrentDrynessIndex = 0.5f; // 0 = wet, 1 = extremely dry
        private float m_AverageTemperature = 0f;
        
        // Weather impact constants
        private const float MAX_DRYNESS = 1.0f;
        private const float MIN_DRYNESS = 0.0f;
        private const float DAILY_DRYNESS_INCREASE = 0.03f; // How much dryness increases per hot, dry day
        private const float RAIN_DRYNESS_REDUCTION = 0.1f; // How much dryness decreases per rainy day
        private const float COLD_WEATHER_THRESHOLD = 5.0f; // Temperature below which fires are less likely
        private const float LIGHTNING_MULTIPLIER = 0.5f; // Lightning can still cause fires even in wet conditions
        
        // Structure to record daily weather
        private struct WeatherRecord
        {
            public float Temperature;
            public float Precipitation;
            public float Dryness;
            
            public WeatherRecord(float temperature, float precipitation, float dryness)
            {
                Temperature = temperature;
                Precipitation = precipitation;
                Dryness = dryness;
            }
        }
        //Building Collapse Toggle
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
        //Building Fire Toggle
        public void TurnOffBuildingFire(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                fireData.m_StartProbability = 0;
                EntityManager.SetComponentData(prefabEntity, fireData);
            }            
        }
        public void TurnOnBuildingFire(PrefabID prefabID)
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
        //Forest Fire Toggle
        public void TurnOffForestFire(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                fireData.m_StartProbability = 0;
                EntityManager.SetComponentData(prefabEntity, fireData);
            }            
        }
        public void TurnOnForestFire(PrefabID prefabID)
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
        
        
        
        public void ControlBuildingFires(PrefabID prefabID)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabID, out PrefabBase prefab)
                && prefab.TryGet(out Game.Prefabs.Fire fire)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.TryGetComponent(prefabEntity, out Game.Prefabs.FireData fireData))
            {
                
                float fireStartIntensity = fire.m_StartIntensity;
                float fireEscalationRate = fire.m_EscalationRate;
                float fireSpreadProbability = fire.m_SpreadProbability;
                float fireSpreadRange = fire.m_SpreadRange;
                
                
                fireData.m_StartIntensity = fireStartIntensity * (Mod.m_Setting.BuildingFireStartIntensity / 100.0f);
                fireData.m_EscalationRate = fireEscalationRate * (Mod.m_Setting.BuildingFireEscalationRate / 100.0f);
                fireData.m_SpreadProbability = fireSpreadProbability * (Mod.m_Setting.BuildingFireSpreadProbability / 100.0f);
                fireData.m_SpreadRange = fireSpreadRange * (Mod.m_Setting.BuildingFireSpreadRange / 100.0f);
                EntityManager.SetComponentData(prefabEntity, fireData);
            }
        }
        
        // Calculate weather-based multipliers for forest fire probabilities
        private float CalculateWeatherFireMultiplier(bool isLightningStrike = false)
        {
            // Skip weather calculations if weather effects are disabled
            if (!Mod.m_Setting.EnableWeatherEffectsOnFires)
            {
                return 1.0f; // Return neutral multiplier when disabled
            }
            
            // Get current weather conditions
            float currentTemp = (float)m_ClimateSystem.temperature;
            float currentPrecipitation = (float)m_ClimateSystem.precipitation;
            bool isRaining = m_ClimateSystem.isRaining;
            bool isSnowing = m_ClimateSystem.isSnowing;
            
            // Update dryness index based on current conditions
            UpdateWeatherHistory(currentTemp, currentPrecipitation);
            
            float seasonMultiplier = ShouldApplySummerFireMultiplier() ? SUMMER_FIRE_MULTIPLIER : 1.0f;
            
            // Weather-based adjustments
            float precipitationFactor = isRaining || isSnowing ? 0.2f : 1.0f; // Heavy reduction during active precipitation
            float temperatureFactor = currentTemp < COLD_WEATHER_THRESHOLD ? 0.5f : 1.0f; // Cold weather reduces fire risk
            float lightningFactor = isLightningStrike ? LIGHTNING_MULTIPLIER : 0.0f; // Lightning can cause fires even in wet conditions
            
            // Combine factors - dryness has the biggest impact
            float weatherMultiplier = math.lerp(0.1f, 1.5f, m_CurrentDrynessIndex);
            
            // Apply precipitation and temperature factors, but allow lightning to override some of these restrictions
            if (isLightningStrike) {
                // Lightning can cause fires even in poor conditions, but still affected by extreme wet/cold
                weatherMultiplier = math.max(lightningFactor, weatherMultiplier * precipitationFactor * temperatureFactor);
            } else {
                weatherMultiplier *= precipitationFactor * temperatureFactor;
            }
            
            // Apply seasonal factor on top of weather
            return weatherMultiplier * seasonMultiplier;
        }
        
        // Update the weather history and recalculate dryness index
        private void UpdateWeatherHistory(float temperature, float precipitation)
        {
            // Skip weather history updates if weather effects are disabled
            if (!Mod.m_Setting.EnableWeatherEffectsOnFires)
            {
                return;
            }
            
            // Ensure we don't store too many records
            while (m_WeatherHistory.Count >= HISTORY_SIZE)
            {
                m_WeatherHistory.Dequeue();
            }
            
            // Calculate new dryness value
            float newDryness = m_CurrentDrynessIndex;
            
            // Increase dryness if hot and dry
            if (temperature > m_AverageTemperature && precipitation < 0.1f)
            {
                newDryness += DAILY_DRYNESS_INCREASE;
            }
            
            // Decrease dryness if raining
            if (precipitation > 0.3f)
            {
                newDryness -= RAIN_DRYNESS_REDUCTION * precipitation;
            }
            
            // Keep dryness in valid range
            newDryness = math.clamp(newDryness, MIN_DRYNESS, MAX_DRYNESS);
            
            // Add current weather to history
            m_WeatherHistory.Enqueue(new WeatherRecord(temperature, precipitation, newDryness));
            
            // Set the current dryness
            m_CurrentDrynessIndex = newDryness;
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
                
                // Check if this is a lightning strike or normal forest fire
                bool isLightningStrike = prefabID.Equals(EventPrefabs.LightningStrikePrefabID);
                float weatherMultiplier = CalculateWeatherFireMultiplier(isLightningStrike);
                
                // Apply different factors based on whether this is a lightning strike
                if (isLightningStrike)
                {
                    // Lightning strikes can still happen even in wet conditions, but with reduced intensity/spread
                    fireData.m_StartProbability = fireStartProbability * weatherMultiplier;
                    fireData.m_StartIntensity = Mod.m_Setting.ForestFireStartIntensity * math.lerp(0.5f, 1.0f, m_CurrentDrynessIndex);
                    fireData.m_EscalationRate = Mod.m_Setting.ForestFireEscalationRate * math.lerp(0.3f, 1.0f, m_CurrentDrynessIndex);
                    fireData.m_SpreadProbability = Mod.m_Setting.ForestFireSpreadProbability * weatherMultiplier * 0.8f;
                    fireData.m_SpreadRange = Mod.m_Setting.ForestFireSpreadRange * math.lerp(0.6f, 1.0f, m_CurrentDrynessIndex);
                }
                else 
                {
                    // Normal forest fires heavily dependent on weather conditions
                    fireData.m_StartProbability = fireStartProbability * weatherMultiplier;
                    fireData.m_StartIntensity = Mod.m_Setting.ForestFireStartIntensity * math.lerp(0.7f, 1.2f, m_CurrentDrynessIndex);
                    fireData.m_EscalationRate = Mod.m_Setting.ForestFireEscalationRate * math.lerp(0.5f, 1.5f, m_CurrentDrynessIndex);
                    fireData.m_SpreadProbability = Mod.m_Setting.ForestFireSpreadProbability * weatherMultiplier;
                    fireData.m_SpreadRange = Mod.m_Setting.ForestFireSpreadRange * math.lerp(0.7f, 1.3f, m_CurrentDrynessIndex);
                }
                
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
            m_LightningStrikeSystem = World.GetOrCreateSystemManaged<LightningStrikeEventSystem>(); // Get LightningStrikeEventSystem
            
            // Initialize with average temperature from climate system
            if (m_ClimateSystem != null)
            {
                m_AverageTemperature = m_ClimateSystem.averageTemperature;
            }
            
            // Reset dryness index when weather effects are disabled
            if (!Mod.m_Setting.EnableWeatherEffectsOnFires)
            {
                m_CurrentDrynessIndex = 0.5f;
                m_WeatherHistory.Clear();
            }
        }
        
        protected override void OnUpdate()
        { 
            // Update forest fire settings every frame to account for season and weather changes
            HandleBFandFFControls(true, EventPrefabs.LightningStrikePrefabID); // Uncommented this line
            HandleBFandFFControls(true, EventPrefabs.BuildingFirePrefabID);
            HandleBFandFFControls(true, EventPrefabs.ForestFirePrefabID);
            HandleBuildingCollapseOccurence(Mod.m_Setting.BuildingCollapseOccurenceToggle, EventPrefabs.BuildingCollapseID);

        }
        
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            HandleBuildingCollapseOccurence(Mod.m_Setting.BuildingCollapseOccurenceToggle, EventPrefabs.BuildingCollapseID);
            HandleBuildingFireOccurence(Mod.m_Setting.BuildingFireToggle, EventPrefabs.BuildingFirePrefabID);
            HandleForestFireOccurence(Mod.m_Setting.ForestFireToggle, EventPrefabs.ForestFirePrefabID);
            HandleBFandFFControls(true, EventPrefabs.BuildingFirePrefabID);
            HandleBFandFFControls(true, EventPrefabs.ForestFirePrefabID);
            
        }
        //Building Collapse
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
        //Building Fire
        private void HandleBuildingFireOccurence(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                TurnOnBuildingFire(prefabID);
            }
            else
            {
                TurnOffBuildingFire(prefabID);
            }
        }
        //Forest Fire
        private void HandleForestFireOccurence(bool toggle, PrefabID prefabID)
        {
            if (!toggle)
            {
                TurnOnForestFire(prefabID);
            }
            else
            {
                TurnOffForestFire(prefabID);
            }
        }
        private void HandleBFandFFControls(bool apply, PrefabID prefabID)
        {
            if (apply)
            {
                // If this is a lightning strike, delegate to LightningStrikeEventSystem
                if (prefabID.Equals(EventPrefabs.LightningStrikePrefabID) && m_LightningStrikeSystem != null)
                {
                    m_LightningStrikeSystem.ControlLightningStrikes(prefabID);
                    m_LightningStrikeSystem.ControlLightningStrikeFires(prefabID);
                }
                else
                {
                    ControlBuildingFires(prefabID);
                    ControlForestFires(prefabID);
                }
            }
        }
    }
}
