using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Banapuchin.Patches;
using UnityEngine;
using Banapuchin.Libraries;
using Il2CppInterop.Runtime.Injection;
using Banapuchin.Classes;
using Locomotion;
using Banapuchin.Mods.Movement;
using System.Reflection;
using System.Linq;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Main
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Init : BasePlugin
    {
        public Harmony HarmonyInstance;
        public Plugin PluginInstance;
        public static Init instance;

        public override void Load()
        {
            HarmonyInstance = HarmonyPatcher.Patch(PluginInfo.GUID);
            instance = this;

            ClassInjector.RegisterTypeInIl2Cpp(typeof(BetterMonoBehaviour));
            var monoTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(BetterMonoBehaviour).IsAssignableFrom(t) && t != typeof(BetterMonoBehaviour)).ToArray();
            foreach (var type in monoTypes)
            {
                ClassInjector.RegisterTypeInIl2Cpp(type);
            }

            PluginInstance = AddComponent<Plugin>();
            AddComponent<OnInit>();
            AddComponent<HapticLibrary>();
            AddComponent<CoroutineManager>();
            AddComponent<ControllerInput>();
        }

        public override bool Unload()
        {
            if (HarmonyInstance != null)
                HarmonyPatcher.Unpatch(HarmonyInstance);

            return true;
        }
    }

    public class OnInit : MonoBehaviour
    {
        private bool hasDone;
        
        void Update()
        {
            if (!hasDone && Player.Instance != null)
            {
                Bouncy.normal = Player.Instance.climbDrag;
                UltraBouncy.normal = Player.Instance.climbDrag;
                bundle = LoadAssetBundle("Banapuchin.Assets.modmenu");
                hasDone = true;
            }
        }
    }
}
