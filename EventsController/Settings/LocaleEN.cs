using Colossal;
using System.Collections.Generic;
using EventsController.Domain;

namespace EventsController.Settings
{
    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;
        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Event Controller" },
                //Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.LightningSection), "Lightning" },
                { m_Setting.GetOptionTabLocaleID(Setting.TornadoSection), "Tornado" },
                {m_Setting.GetOptionTabLocaleID(Setting.BuildingSection), "Building and Forest" },
                {m_Setting.GetOptionTabLocaleID(Setting.HailStormGroup), "Hail Storms" },
                {m_Setting.GetOptionTabLocaleID(Setting.ResetSection), "Reset" },
                {m_Setting.GetOptionTabLocaleID(Setting.OtherSection), "Other" },
                {m_Setting.GetOptionTabLocaleID(Setting.SeasonalEventsSection), "Seasonal Events " },
                { m_Setting.GetOptionTabLocaleID(Setting.AboutSection), "About" },
                //Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.WPGroup), "Weather Phenomenons" },
                { m_Setting.GetOptionGroupLocaleID(Setting.FireGroup), "Fires" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AccidentGroup), "Traffic Accidents" },
                { m_Setting.GetOptionGroupLocaleID(Setting.BuildingCollapseGroup), "Building Collapse" },
                { m_Setting.GetOptionGroupLocaleID(Setting.BuildingFiresGroup), "Building Fires" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ForestFireGroup), "Forest Fires" },
                { m_Setting.GetOptionGroupLocaleID(Setting.RobberyGroup), "Robberies" },
                { m_Setting.GetOptionGroupLocaleID(Setting.LooseControlAccidentGroup), "Loose Control Accidents" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ResetGroup), "Resetting" },
                { m_Setting.GetOptionGroupLocaleID(Setting.AboutGroup), "About" },
                //Lighting Strikes
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningStrikeOccurenceToggle)), "Lightning Strike Occurrences" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningStrikeOccurenceToggle)), "Toggle whether lightning strikes can occur in your city. When enabled, lightning strikes will happen based on defined parameters." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningIntervalMin)), "Lightning Interval Min" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningIntervalMin)), "Minimum time (in seconds) between consecutive lightning strikes. Lower values increase frequency." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningIntervalMax)), "Lightning Interval Max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningIntervalMax)), "Maximum time (in seconds) between consecutive lightning strikes. Higher values decrease frequency." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DurationMin)), "Duration Minimum" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DurationMin)), "Minimum duration (in seconds) of a lightning storm event. Affects how long the event lasts." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.DurationMax)), "Duration Maximum" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.DurationMax)), "Maximum duration (in seconds) of a lightning storm event. Sets the upper limit for event length." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OccurenceTemperatureMin)), "Occurence Temperature Min" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OccurenceTemperatureMin)), "Minimum temperature threshold required for lightning strikes to occur. Affects seasonal occurrence." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OccurenceTemperatureMax)), "Occurence Temperature Max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OccurenceTemperatureMax)), "Maximum temperature threshold  at which lightning strikes can occur. Sets upper temperature boundary." },
                //Lighting Strike Fires
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningFireStartProbability)), "Lightning Fire Start Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningFireStartProbability)), "Probability percentage that a lightning strike will ignite a fire. 100% represents default game behavior." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningFireStartIntensity)), "Lightning Fire Start Intensity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningFireStartIntensity)), "Initial intensity of fires caused by lightning strikes. Higher values create more dangerous initial fires." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningFireEscalationRate)), "Lightning Fire Escalation Rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningFireEscalationRate)), "How quickly lightning-caused fires grow in intensity. Higher values make fires escalate faster." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningFireSpreadProbability)), "Lightning Fire Spread Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningFireSpreadProbability)), "Chance percentage that lightning-caused fires will spread to adjacent areas. Controls fire expansion." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LightningFireSpreadRange)), "Lightning Fire Spread Range" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LightningFireSpreadRange)), "Maximum distance lightning-caused fires can spread. Larger values allow fires to affect wider areas." },
                //Tornado
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoOccurenceToggle)), "Tornado Occurrences" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoOccurenceToggle)), "Toggle whether tornadoes can form in your city" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoDurationMin)), "Tornado Duration Min" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoDurationMin)), "Minimum duration (in seconds) that a tornado will remain active. Affects disaster planning time." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoDurationMax)), "Tornado Duration Max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoDurationMax)), "Maximum duration (in seconds) that a tornado can remain active. Limits maximum destruction time." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoOccurenceTemperatureMin)), "Tornado Occurence Temperature Min" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoOccurenceTemperatureMin)), "Minimum temperature  required for tornadoes to form" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoOccurenceTemperatureMax)), "Tornado Occurence Temperature Max" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoOccurenceTemperatureMax)), "Maximum temperature  at which tornadoes can form. Determines upper temperature boundary." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoOccurenceRainMin)), "Tornado Occurence Rain Min" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoOccurenceRainMin)), "Minimum rain intensity required for tornadoes to form." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoOccurenceRainMax)), "Tornado Occurence Rain Max" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoDamageSeverity)), "Tornado Damage Severity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoDamageSeverity)), "The damage severity of tornado's" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TornadoTrafficAccidentOccurenceProbability)), "Tornado Traffic Accident Occurence Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TornadoTrafficAccidentOccurenceProbability)), "Chance percentage for vehicle accidents during tornado events." },
                //Building related Settings
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingCollapseOccurenceToggle)), "Building Collapse " },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingCollapseOccurenceToggle)), "Toggle whether buildings can collapse" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingFireStartProbability)), "Building Fire Start Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingFireStartProbability)), "Chance percentage for fires to spontaneously start in buildings" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingFireStartIntensity)), "Building Fire Start Intensity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingFireStartIntensity)), "Initial intensity of building fires." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingFireEscalationRate)), "Building Fire Escalation Rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingFireEscalationRate)), "How quickly building fires grow in intensity." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingFireSpreadProbability)), "Building Fire Spread Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingFireSpreadProbability)), "Chance percentage that building fires will spread to adjacent structures." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.BuildingFireSpreadRange)), "Building Fire Spread Range" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.BuildingFireSpreadRange)), "Maximum distance building fires can spread to nearby structures." },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ForestFireStartProbability)), "Forest Fire Start Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ForestFireStartProbability)), "Chance for spontaneous forest fires to start." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ForestFireStartIntensity)), "Forest Fire Start Intensity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ForestFireStartIntensity)), "Initial intensity of forest fires." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ForestFireEscalationRate)), "Forest Fire Escalation Rate" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ForestFireEscalationRate)), "How quickly forest fires grow once started." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ForestFireSpreadProbability)), "Forest Fire Spread Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ForestFireSpreadProbability)), "Chance that forest fires will expand to nearby wooded areas." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ForestFireSpreadRange)), "Forest Fire Spread Range" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ForestFireSpreadRange)), "Maximum distance forest fires can spread." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.EnableSummerFireIncrease)), "Summer fires" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.EnableSummerFireIncrease)), "When enabled, forest fire probability increases during summer month" },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OccurenceProbabilityMin)), "Minimum Occurence Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OccurenceProbabilityMin)), "Minimum chance percentage for robberies to occur. Sets the lower probability boundary." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.OccurenceProbabilityMax)), "Maximum Occurence Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.OccurenceProbabilityMax)), "Maximum chance percentage for robberies to occur. Sets the upper probability boundary." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RecurrenceProbabilityMin)), "Minimum Recurrence Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RecurrenceProbabilityMin)), "Minimum chance percentage for repeat robberies at the same location." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RecurrenceProbabilityMax)), "Maximum Recurrence Probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.RecurrenceProbabilityMax)), "Maximum chance percentage for repeat robberies." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LCAFireStartProbabilityToggle)), "Fire probability" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LCAFireStartProbabilityToggle)), "Toggle whether vehicle accidents can cause fires. When enabled, crashes may lead to vehicle fires." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.LCAAccidentOccurenceToggle)), "Occurrences" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.LCAAccidentOccurenceToggle)), "Toggle whether vehicles can lose control and cause accidents." },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HsOccurenceToggle)), "Hail Storm Occurrences " },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HsOccurenceToggle)), "Toggle whether hail storms can occur in your city." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSDamageSeverity)), "Hail Storm Damage Severity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSDamageSeverity)), "Destruction power of hail storms. Higher values increase potential damage structures." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSDurationMin)), "Minimum Duration" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSDurationMin)), "Minimum duration (in seconds) of hail storm events." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSDurationMax)), "Maximum Duration" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSDurationMax)), "Maximum duration (in seconds) of hail storm events. Limits the upper time boundary of storms." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSTemperatureMin)), "Minimum Temperature" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSTemperatureMin)), "Minimum temperature required for hail storms to form." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSTemperatureMax)), "Maximum Temperature" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSTemperatureMax)), "Maximum temperature at which hail storms can form." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSRainMin)), "Minimum Rain" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSRainMin)), "Minimum rain intensity required for hail storms." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSRainMax)), "Maximum Rain" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSRainMax)), "Maximum rain intensity during which hail storms can occur." },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HSTrafficAccidentOccurenceProbability)), "Traffic Accident Occurence" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HSTrafficAccidentOccurenceProbability)), "Chance percentage for vehicle accidents during hail storms. Higher values increase accident frequency." },
                
                //Resetting
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetModSettings)), "Reset All" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetModSettings)), "Reset all Event Controller settings to their default values. This will undo all your customizations." },
                //About
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Version)), "Version" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.Version)), "Current version number of the Event Controller mod. Useful for troubleshooting and reporting issues." },
            };
        }

        public void Unload()
        {

        }
    }
}