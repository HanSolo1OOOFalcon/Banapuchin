using Banapuchin.Classes;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Banapuchin
{
    internal class PublicThingsHerePlease
    {
        public static GameObject menu;
        public static GameObject rBall, lBall;

        public static bool allowed { get; internal set; }

        public static int currentPage = 0;

        public static List<ModBase> modInstances = new List<ModBase>();

        public static void UpdateButtons()
        {
            int currentButtonIndex = 0;
            foreach (var button in modInstances)
            {
                int startIndex = currentPage * 5;
                int endIndex = startIndex + 5;

                bool isVisible = currentButtonIndex >= startIndex && currentButtonIndex < endIndex;

                button.ButtonObject.SetActive(isVisible);
                button.TextObject.SetActive(isVisible);

                currentButtonIndex++;
            }
        }

        public static Texture2D LoadTexture(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    Debug.LogError($"Resource '{path}' not found.");
                    return null;
                }

                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);

                return texture;
            }
        }

        // the following method will stay here as it saved me from a lot of headaches while monky figured out asset bundles but now that assetbundles are a thing this method is deprecated
        // nvm isnt deprecated got the following error:
        /*
        [Error  :Il2CppInterop] During invoking native->managed trampoline
        Exception: System.IO.FileNotFoundException: Could not load file or assembly 'MelonLoader, Version=0.6.4.0, Culture=neutral, PublicKeyToken=null'. Det går inte att hitta filen.
        File name: 'MelonLoader, Version=0.6.4.0, Culture=neutral, PublicKeyToken=null'
            at UnityEngine.Il2CppAssetBundle.LoadAsset[T](String name)
            at Banapuchin.Main.Plugin.CreateMenu()
            at Banapuchin.Main.Plugin.OnModdedJoin()
            at Caputilla.CaputillaManager.InvokeModdedJoin()
            at Caputilla.Utils.RoomUtils.OnJoin()
            at CapuchinTemplate.Patches.FusionHubPatchesJoin.Postfix(FusionHub __instance)
            at DMD<FusionHub::JoinedRoom>(FusionHub this, SessionInfo info)
            at (il2cpp -> managed) JoinedRoom(IntPtr , IntPtr , Il2CppMethodInfo* )
        */
        public static void CreateBanana(out GameObject banana, int segmentCount = 24, float arcRadius = 0.6f, float totalCurveAngle = 60f, float baseRadius = 0.1f, float segmentLength = 0.15f)
        {
            Color bananaYellow = new Color32(251, 255, 135, 255);
            banana = new GameObject("Banana");

            float angleStep = totalCurveAngle / (segmentCount - 1);

            for (int i = 0; i < segmentCount; i++)
            {
                GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                segment.name = $"Segment_{i + 1}";
                segment.transform.SetParent(banana.transform);

                float angle = -totalCurveAngle / 2f + i * angleStep;
                float rad = Mathf.Deg2Rad * angle;

                float x = Mathf.Cos(rad) * arcRadius;
                float y = Mathf.Sin(rad) * arcRadius;

                segment.transform.localPosition = new Vector3(x, y, 0f);
                segment.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

                float taper = Mathf.Lerp(0.4f, 1f, Mathf.Sin(Mathf.PI * i / segmentCount));
                segment.transform.localScale = new Vector3(baseRadius * taper, segmentLength / 2f, baseRadius * taper);

                Renderer r = segment.GetComponent<Renderer>();
                r.material.shader = Shader.Find("Unlit/Color");
                r.material.color = bananaYellow;

                GameObject.Destroy(segment.GetComponent<Collider>());
            }
        }
    }
}