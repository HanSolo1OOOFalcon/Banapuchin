using System.Text;
using Banapuchin.Classes;
using Banapuchin.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppFusion;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using UnityEngine;

namespace Banapuchin.Patches
{
    [HarmonyPatch(typeof(FusionPlayer), "Spawned")]
    public class FusionPlayerSpawnedPatch
    {
        public static bool ShouldESP;
        private const string MyUserID = "6EDAB722E8A7851D";

        private static void Postfix(FusionPlayer __instance)
        {
            if (ShouldESP && !__instance.IsLocalPlayer)
            {
                __instance.skin.material.color = __instance.__Color;
                __instance.skin.material.shader = Shader.Find("GUI/Text Shader");
            }

            if (!__instance.IsLocalPlayer && FusionHub.Runner.GetPlayerUserId(__instance.playerreal) == MyUserID)
            {
                GameObject gadubudaHat = UnityEngine.Object.Instantiate(
                    original: PublicThingsHerePlease.bundle.LoadAsset<GameObject>("GadubudaHat"),
                    parent: __instance.transform.Find("Head").Find("headTarget").Find("PlayerModel").Find("CapuchinRemade").Find("Capuchin").Find("torso").Find("head")
                );
                
                MelonLogger.Msg(gadubudaHat.GetHierarchyPath());
                
                gadubudaHat.name = "GadubudaHat";
                gadubudaHat.transform.localPosition = new Vector3(0f, 0.003f, -0.0003f);
                gadubudaHat.transform.localScale = Vector3.one * 0.14f;
                gadubudaHat.transform.localRotation = Quaternion.Euler(284f, 180f, 180f);

                foreach (Material mat in gadubudaHat.GetComponent<Renderer>().materials)
                    mat.shader = Shader.Find("Shader Graphs/ShadedPiss");

                gadubudaHat.GetComponent<Renderer>().materials[0].mainTexture = PublicThingsHerePlease.bundle.LoadAsset<Texture2D>("HatPurpleBaked");
            }
        }
    }
}