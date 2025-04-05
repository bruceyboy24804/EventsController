using Colossal.IO.AssetDatabase;
using EventsController.Domain;
using EventsController.Systems;
using Game.City;
using Game.Modding;
using Game.Prefabs;
using Game.Serialization;
using Game.Settings;
using Game.UI;
using Unity.Entities;
using Unity.Mathematics;
using static Game.Prefabs.CompositionFlags;

namespace EventsController.Settings
{
    [FileLocation(nameof(EventsController))]
    [SettingsUITabOrder(LightningSection, TornadoSection, HailStormSection ,BuildingSection, OtherSection, SeasonalEventsSection ,ResetSection ,AboutSection)]
    [SettingsUIGroupOrder(WPGroup, FireGroup, AccidentGroup, BuildingCollapseGroup, BuildingFiresGroup, ForestFireGroup, HailStormGroup, RobberyGroup, LooseControlAccidentGroup, SeasonalEventsGroup ,ResetGroup, AboutGroup)]
    [SettingsUIShowGroupName(WPGroup, FireGroup, AccidentGroup, BuildingCollapseGroup, BuildingFiresGroup, ForestFireGroup, HailStormGroup, RobberyGroup, LooseControlAccidentGroup ,ResetGroup, AboutGroup)]
    
    public class Setting : ModSetting
    {
        public const string LightningSection = "Lightning";
        public const string AboutSection = "About";
        public const string TornadoSection = "Tornadoes";
        public const string HailStormSection = "Hail Storms";
        public const string BuildingSection = "Buildings & Forest";
        public const string OtherSection = "Other";
        public const string SeasonalEventsSection = "Seasonal Events";
        public const string ResetSection = "Reset";
        
        public const string WPGroup = "Weather Phenomenons";
        public const string FireGroup = "Fires";
        public const string AccidentGroup = "Traffic Accidents";
        public const string BuildingCollapseGroup = "Building Collapse";
        public const string BuildingFiresGroup = "Building Fires";
        public const string ForestFireGroup = "Forest Fires";
        public const string HailStormGroup = "Hail Storms";
        public const string RobberyGroup = "Robberies";
        public const string LooseControlAccidentGroup = "Lose Control Accidents";
        public const string ResetGroup = "Resetting";
        public const string SeasonalEventsGroup = "Seasonal Events";
        public const string AboutGroup = "About";
        
        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        #region Lightning Settings
        //Lighting Strikes
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISetter(typeof(Setting), nameof(ToggleLightningStrikeOccurences))]
        public bool LightningStrikeOccurenceToggle { get; set; }
        
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 360f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningIntervalMin { get; set; }
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 360f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningIntervalMax { get; set; }
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 120, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float DurationMin { get; set; }
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 120f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float DurationMax { get; set; }
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float OccurenceTemperatureMin {get; set; }
        
        
        [SettingsUISection(LightningSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float OccurenceTemperatureMax { get; set; }
        
        //Lighting Strike Fires
        [SettingsUISection(LightningSection, FireGroup)]
        [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
        [SettingsUISetter(typeof(Setting), nameof(HandleLightningControl))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningFireStartProbability {get; set; }
        
        [SettingsUISection(LightningSection, FireGroup)]
        [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
        [SettingsUISetter(typeof(Setting), nameof(HandleLightningControl))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningFireStartIntensity { get; set; }
        
        [SettingsUISection(LightningSection, FireGroup)]
        [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
        [SettingsUISetter(typeof(Setting), nameof(HandleLightningControl))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningFireEscalationRate { get; set; }
        
        [SettingsUISection(LightningSection, FireGroup)]
        [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
        [SettingsUISetter(typeof(Setting), nameof(HandleLightningControl))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningFireSpreadProbability { get; set; }
        
        [SettingsUISection(LightningSection, FireGroup)]
        [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
        [SettingsUISetter(typeof(Setting), nameof(ControlLightningControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(LightningStrikeOccurenceToggle), true)]
        public float LightningFireSpreadRange { get; set; }
        #endregion
        
       #region Tornado Settings
        //Tornadoes
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        public bool TornadoOccurenceToggle { get; set; }
        
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 1000f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoDamageSeverity { get; set; }
        public float TornadoDurationMin { get; set; }
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 360f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]

        public float TornadoDurationMax { get; set; }
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoOccurenceTemperatureMin { get; set; }
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoOccurenceTemperatureMax { get; set; }
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoOccurenceRainMin { get; set; }
        [SettingsUISection(TornadoSection, WPGroup)]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoOccurenceRainMax { get; set; }
        //Tornado Traffic Accidents
        [SettingsUISection(TornadoSection, AccidentGroup)]
        [SettingsUISetter(typeof(Setting), nameof(ControlTornadoControls))]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
        [SettingsUIHideByCondition(typeof(Setting), nameof(TornadoOccurenceToggle), true)]
        public float TornadoTrafficAccidentOccurenceProbability { get; set; }
       #endregion
       
       #region Building and Forest Related Settings
       [SettingsUISection(BuildingSection, BuildingCollapseGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleBuildingCollapseOccurences))]
       public bool BuildingCollapseOccurenceToggle { get; set; }
       
       [SettingsUISection(BuildingSection, BuildingFiresGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleBuildingFires))]
       public bool BuildingFireToggle { get; set; }
       
       [SettingsUISection(BuildingSection, BuildingFiresGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
       [SettingsUIHideByCondition(typeof(Setting), nameof(BuildingFireToggle), true)]
       public float BuildingFireStartIntensity { get; set; }
       [SettingsUISection(BuildingSection, BuildingFiresGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
       [SettingsUIHideByCondition(typeof(Setting), nameof(BuildingFireToggle), true)]
       public float BuildingFireEscalationRate { get; set; }
       [SettingsUISection(BuildingSection, BuildingFiresGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
       [SettingsUIHideByCondition(typeof(Setting), nameof(BuildingFireToggle), true)]
       public float BuildingFireSpreadProbability { get; set; }
       
       [SettingsUISection(BuildingSection, BuildingFiresGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 200f, step = 1f, unit = Unit.kPercentage)]
       [SettingsUIHideByCondition(typeof(Setting), nameof(BuildingFireToggle), true)]
       public float BuildingFireSpreadRange { get; set; }
       
       [SettingsUISection(BuildingSection, ForestFireGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleForestFires))]
         public bool ForestFireToggle { get; set; }
       [SettingsUISection(BuildingSection, ForestFireGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
         [SettingsUIHideByCondition(typeof(Setting), nameof(ForestFireToggle), true)]
       public float ForestFireStartIntensity { get; set; }
       [SettingsUISection(BuildingSection, ForestFireGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(ForestFireToggle), true)]
       public float ForestFireEscalationRate { get; set; }
       [SettingsUISection(BuildingSection, ForestFireGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kFloatSingleFraction)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(ForestFireToggle), true)]
       public float ForestFireSpreadProbability { get; set; }
       [SettingsUISection(BuildingSection, ForestFireGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(ForestFireToggle), true)]
       public float ForestFireSpreadRange { get; set; }
       #endregion
        [SettingsUISection(OtherSection, RobberyGroup)]
        [SettingsUISetter(typeof(Setting), nameof(ToggleRobberyOccurrences))]
        public bool RobberyOccurenceToggle { get; set; }
       [SettingsUISection(OtherSection, RobberyGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleRobberyOccurrences))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
         [SettingsUIHideByCondition(typeof(Setting), nameof(RobberyOccurenceToggle), true)]

       public float OccurenceProbabilityMin { get; set; }
        
       [SettingsUISection(OtherSection, RobberyGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleRobberyOccurrences))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(RobberyOccurenceToggle), true)]
       public float OccurenceProbabilityMax { get; set; }
       
       [SettingsUISection(OtherSection, RobberyGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleRobberyOccurrences))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(RobberyOccurenceToggle), true)]
       public float RecurrenceProbabilityMin { get; set; }
        
       [SettingsUISection(OtherSection, RobberyGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleRobberyOccurrences))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(RobberyOccurenceToggle), true)]
       public float RecurrenceProbabilityMax { get; set; }
       
       
       [SettingsUISection(OtherSection, LooseControlAccidentGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleTAOccurences))]
       public bool LCAAccidentOccurenceToggle { get; set; }
       
       [SettingsUISection(OtherSection, LooseControlAccidentGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleLCAFireProbability))]
       [SettingsUIHideByCondition(typeof(Setting), nameof(LCAAccidentOccurenceToggle), true)]
       public bool LCAFireStartProbabilityToggle { get; set; }
        
       
       
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ToggleHSOccurences))]
       public bool HsOccurenceToggle { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
         [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSDamageSeverity { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSDurationMin { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSDurationMax { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSTemperatureMin { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSTemperatureMax { get; set; }
       
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 1f, step = 0.1f, unit = Unit.kFloatSingleFraction)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSRainMin { get; set; }
       [SettingsUISection(HailStormSection, WPGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 1f, step = 0.1f, unit = Unit.kFloatSingleFraction)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSRainMax { get; set; }
       [SettingsUISection(HailStormSection, AccidentGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlHSControls))]
       [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = Unit.kInteger)]
            [SettingsUIHideByCondition(typeof(Setting), nameof(HsOccurenceToggle), true)]
       public float HSTrafficAccidentOccurenceProbability { get; set; }
       
       [SettingsUISection(SeasonalEventsSection, SeasonalEventsGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       public bool EnableSummerFireIncrease { get; set; } 
       [SettingsUISection(SeasonalEventsSection, SeasonalEventsGroup)]
       [SettingsUISetter(typeof(Setting), nameof(ControlBFandFFControls))]
       public bool EnableWeatherEffectsOnFires { get; set; }  
       
        [SettingsUIButton]
        [SettingsUISection(ResetSection, ResetGroup)]
        public bool ResetModSettings
        {
            set
            {
                SetDefaults();
                ApplyAndSave();
            }
        }
        

        /// <summary>
        /// Gets a value indicating the version.
        /// </summary>
        [SettingsUISection(AboutGroup, AboutSection)]
        public string Version => Mod.Version;

        
        public override void SetDefaults()
        {
            
            LightningStrikeOccurenceToggle = true;
            LightningIntervalMin = 60f;
            LightningIntervalMax = 60f;
            DurationMin = 30f;
            DurationMax = 60f;
            OccurenceTemperatureMin = 10f;
            OccurenceTemperatureMax = 50f;
            LightningFireStartProbability = 100f;
            LightningFireStartIntensity = 100f;
            LightningFireEscalationRate = 100f;
            LightningFireSpreadProbability = 100f;
            LightningFireSpreadRange = 100f;
            
            TornadoOccurenceToggle = true;
            TornadoDamageSeverity = 2000f;
            TornadoDurationMin = 60f;
            TornadoDurationMax = 360f;
            TornadoOccurenceTemperatureMin = 10f;
            TornadoOccurenceTemperatureMax = 30f;
            TornadoOccurenceRainMin = 0f;
            TornadoOccurenceRainMax = 1f;
            TornadoTrafficAccidentOccurenceProbability = 10f;
            
            
            BuildingCollapseOccurenceToggle = true;
            BuildingFireToggle = true;
            BuildingFireStartIntensity = 100f;
            BuildingFireEscalationRate = 100f;
            BuildingFireSpreadProbability = 100f;
            BuildingFireSpreadRange = 100f;
            ForestFireToggle = true;
            ForestFireStartIntensity = 1f;
            ForestFireEscalationRate = 1.666667f;
            ForestFireSpreadProbability = 1.5f;
            ForestFireSpreadRange = 30f;
            EnableSummerFireIncrease = false;
            EnableWeatherEffectsOnFires = false;
            
            RobberyOccurenceToggle = true;
            OccurenceProbabilityMin = 1f;
            OccurenceProbabilityMax = 5f;
            RecurrenceProbabilityMin = 1f;
            RecurrenceProbabilityMax = 7f;
            LCAFireStartProbabilityToggle = true;
            LCAAccidentOccurenceToggle = true;
            
            HsOccurenceToggle = true;
            HSDamageSeverity = 10f;
            HSDurationMin = 15f;
            HSDurationMax = 90f;
            HSTemperatureMin = 0f;
            HSTemperatureMax = 15f;
            HSRainMin = 0f;
            HSRainMax = 1f;
            HSTrafficAccidentOccurenceProbability = 1f;
            
            
        }
        
       
        
        
        
        
        public void ToggleLCAFireProbability(bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            ToggleFireoccurences(EventPrefabs.LoseControlAccidentPrefabID, state);
        }
        public void ToggleFireoccurences(PrefabID prefabID, bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            if (!state)
            {
                otherEventsSystem.LCAFirestartProbabilityToZero(prefabID);
            }
            else
            {
                otherEventsSystem.ResetLCAFireStartProbability(prefabID);
            }
           
        }
        public void ToggleTAOccurences(bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            ToggleTAOccurence(EventPrefabs.LoseControlAccidentPrefabID, state);
        }
        public void ToggleTAOccurence(PrefabID prefabID, bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            if (!state)
            {
                otherEventsSystem.LCAAccidentOccurenceToZero(prefabID);
            }
            else
            {
                otherEventsSystem.ResetLCAAccidentOccurence(prefabID);
            }
        }
       public void ToggleRobberyOccurrences(bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            ToggleRobberyOccurrence(EventPrefabs.RobberyID, state);
        }

        public void ToggleRobberyOccurrence(PrefabID prefabID, bool state)
        {
            OtherEventsSystem otherEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            if (!state)
            {
                otherEventsSystem.RobberyOccurenceToZero(prefabID);
            }
            else
            {
                otherEventsSystem.ResetRobberyOccurence(prefabID);
                otherEventsSystem.RobberyController(prefabID);
            }
        }
        public void ToggleBuildingCollapseOccurences(bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            ToggleBuildingCollapseOccurence(EventPrefabs.BuildingCollapseID, state);
        }
        public void ToggleBuildingCollapseOccurence(PrefabID prefabID, bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            if (!state)
            {
                buildingAndForestEventsSystem.BuildingCollapseOccurenceToZero(prefabID);
            }
            else
            {
                buildingAndForestEventsSystem.ResetBuildingCollapseOccurences(prefabID);
            }
            
        }
        public void ToggleLightningStrikeOccurences(bool state)
        {
            LightningStrikeEventSystem lightningStrikeEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<LightningStrikeEventSystem>();
            ToggleLightningOccurence(EventPrefabs.LightningStrikePrefabID, state);
        }
        
        public void ToggleLightningOccurence(PrefabID prefabID, bool state)
        {
            LightningStrikeEventSystem lightningStrikeEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<LightningStrikeEventSystem>();
            if (!state)
            {
                lightningStrikeEventSystem.LightningStrikeOccurenceToZero(prefabID);
            }
            else
            {
                lightningStrikeEventSystem.ResetLightningStrikeOccurence(prefabID);
            }
            
        }
        public void ControlLightningControls(bool state)
        {
            LightningStrikeEventSystem lightningStrikeEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<LightningStrikeEventSystem>();
            HandleLightningControl(EventPrefabs.LightningStrikePrefabID, state);
        }
        public void HandleLightningControl(PrefabID prefabID, bool state)
        {
            LightningStrikeEventSystem lightningStrikeEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<LightningStrikeEventSystem>();
            if (state)
            {
                lightningStrikeEventSystem.ControlLightningStrikes(prefabID);
                lightningStrikeEventSystem.ControlLightningStrikeFires(prefabID);
                
            }
            
        }
        public void ControlTornadoControls(bool state)
        {
            TornadoEventSystem tornadoEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<TornadoEventSystem>();
            HandleTornadoControl(EventPrefabs.TornadoPrefabID, state);
        }
        public void HandleTornadoControl(PrefabID prefabID, bool state)
        {
            TornadoEventSystem tornadoEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<TornadoEventSystem>();
            if (state)
            {
                tornadoEventSystem.ControlTornadoes(prefabID);
                tornadoEventSystem.ControlTornadoTrafficAccidents(prefabID);
                
            }
        }
        public void ToggleBuildingFires(bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            ToggleBuildingFire(EventPrefabs.BuildingFirePrefabID, state);
        }
        public void ToggleBuildingFire(PrefabID prefabID, bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            if (!state)
            {
                buildingAndForestEventsSystem.TurnOnBuildingFire(prefabID);
            }
            else
            {
                buildingAndForestEventsSystem.TurnOffBuildingFire(prefabID);
            }
        }
        public void ToggleForestFires(bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            ToggleForestFire(EventPrefabs.ForestFirePrefabID, state);
        }
        public void ToggleForestFire(PrefabID prefabID, bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            if (!state)
            {
                buildingAndForestEventsSystem.TurnOnForestFire(prefabID);
            }
            else
            {
                buildingAndForestEventsSystem.TurnOffForestFire(prefabID);
            }
        }
        public void ControlBFandFFControls(bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            HandleBFandFFControl(EventPrefabs.BuildingFirePrefabID, state);
            HandleBFandFFControl(EventPrefabs.ForestFirePrefabID, state);
        }
        public void HandleBFandFFControl(PrefabID prefabID, bool state)
        {
            BuildingAndForestEventsSystem buildingAndForestEventsSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            if (state)
            {
                buildingAndForestEventsSystem.ControlBuildingFires(prefabID);
                buildingAndForestEventsSystem.ControlForestFires(prefabID);
                
            }
        }
        public void ToggleHSOccurences(bool state)
        {
           HailStormEventSystem hailStormEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<HailStormEventSystem>();
           ToggleHSOccurence(EventPrefabs.HailStormID, state);
        }
        
        public void ToggleHSOccurence(PrefabID prefabID, bool state)
        {
            HailStormEventSystem hailStormEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<HailStormEventSystem>();
            if (!state)
            {
                hailStormEventSystem.HSOccurenceToZero(prefabID);
            }
            else
            {
                hailStormEventSystem.ResetHSOccurence(prefabID);
            }
        }
        public void ControlHSControls(bool state)
        {
            HailStormEventSystem hailStormEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<HailStormEventSystem>();
            HandleHSControl(EventPrefabs.HailStormID, state);
        }
        public void HandleHSControl(PrefabID prefabID, bool state)
        {
            HailStormEventSystem hailStormEventSystem = World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<HailStormEventSystem>();
            if (state)
            {
                hailStormEventSystem.ControlHS(prefabID);
                hailStormEventSystem.ControlHSTrafficAccidents(prefabID);
            }
        }
    }

    
}
