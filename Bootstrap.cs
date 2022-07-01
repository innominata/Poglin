using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Poglin
{

    [BepInPlugin("poglin.cat.xray", "Poglin Plugin", "1.2.0")]
    public class Bootstrap : BaseUnityPlugin
    {
        public new static ManualLogSource Logger;
        public static Queue buffer = new();

        internal void Awake()
        {
            InitializeLogger();
            InitializeComponents();
            ApplyHarmonyPatches();
        }

        private void InitializeLogger()
        {
            var v = Assembly.GetExecutingAssembly().GetName().Version;
            Poglin.Version = $"{v.Major}.{v.Minor}.{v.Build}";
            Logger = new ManualLogSource("Poglin");
            BepInEx.Logging.Logger.Sources.Add(Logger);
        }
        private void InitializeComponents()
        {
            Poglin.speed = Config.Bind("Tweaks", "Speed", 1f, "Override Base Gamespeed");
            Poglin.textReplace = Config.Bind("Tweaks", "Pog", true, "Replace Peglin with Poglin");
            Poglin.bombCounterEnabled = Config.Bind("Tweaks", "Bomb Counter", true, "Enable Bomb Counter");
            // Poglin.CreateBombCountGameObject();
        }
        private void ApplyHarmonyPatches()
        {
            var _ = new Harmony("poglin.cat.xray");
            Harmony.CreateAndPatchAll(typeof(Poglin));
        }

        public static void Debug(object data, LogLevel logLevel, bool isActive)
        {
            if (isActive && Logger != null)
                {
                    while (buffer.Count > 0)
                    {
                        var o = buffer.Dequeue();
                        var l = ((object data, LogLevel loglevel, bool isActive))o;
                        if (l.isActive) Logger.Log(l.loglevel, "Q:" + l.data);
                    }

                    Logger.Log(logLevel, data);
                }
                else
                {
                    buffer.Enqueue((data, logLevel, true));
                }
            
        }

        public static void Debug(object data)
        {
            Debug(data, LogLevel.Message, true);
        }


    }
}