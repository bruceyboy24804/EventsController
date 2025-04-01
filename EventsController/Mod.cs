using System.Reflection;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;
using EventsController.Settings;
using EventsController.Systems;
using Unity.Entities;

namespace EventsController
{
    public class Mod : IMod
    {
        
        public static ILog log = LogManager.GetLogger($"{nameof(EventsController)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        public static Setting m_Setting;
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));


            AssetDatabase.global.LoadSettings(nameof(EventsController), m_Setting, new Setting(this));
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<LightningStrikeEventSystem>();
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<TornadoEventSystem>();
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<BuildingAndForestEventsSystem>();
            World.DefaultGameObjectInjectionWorld?.GetOrCreateSystemManaged<OtherEventsSystem>();
            updateSystem.UpdateAt<LightningStrikeEventSystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<TornadoEventSystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<BuildingAndForestEventsSystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<OtherEventsSystem>(SystemUpdatePhase.MainLoop);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}