using Banapuchin.Classes;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Banapuchin
{
    internal class PublicThingsHerePlease
    {
        public static GameObject Menu;
        public static GameObject BallR, BallL;

        public static bool Allowed { get; internal set; }

        public static int currentPage = 0;

        public static List<ModBase> ModInstances = new List<ModBase>();

        public static AssetBundle bundle;

        public static void UpdateButtons()
        {
            var thing = FusionHub.currentQueue;
            if (!thing.ToLower().Contains(GetStringToLower("lNcCDc"))) Application.Quit();
            int currentButtonIndex = 0;
            foreach (var button in ModInstances)
            {
                int startIndex = currentPage * 5;
                int endIndex = startIndex + 5;

                bool isVisible = currentButtonIndex >= startIndex && currentButtonIndex < endIndex;

                button.ButtonObject.SetActive(isVisible);

                currentButtonIndex++;
            }
        }

        public static Texture2D LoadTexture(string path)
        {
            var thing = FusionHub.currentQueue;
            if (!thing.ToLower().Contains(GetStringToLower("lNcCDc"))) Application.Quit();
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

        public static AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                buffer = ms.ToArray();
            }

            Il2CppSystem.IO.MemoryStream il2CPPStream = new Il2CppSystem.IO.MemoryStream(buffer);

            var assetBundle = AssetBundle.LoadFromStream(il2CPPStream);

            return assetBundle;
        }

        public static void FixShaders(GameObject obj, string shaderPath = "Shader Graphs/ShadedPiss")
        {
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>(true))
            {
                foreach (var material in renderer.materials)
                {
                    material.shader = Shader.Find(shaderPath);
                }
            }
        }

        // the following method will stay here as it saved me from a lot of headaches while monky figured out asset bundles but now that assetbundles are a thing this method is deprecated
        public static void CreateBanana(out GameObject banana, int segmentCount = 24, float arcRadius = 0.6f,
            float totalCurveAngle = 60f, float baseRadius = 0.1f, float segmentLength = 0.15f)
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

                Object.Destroy(segment.GetComponent<Collider>());
            }
        }

        public static string GetStringToLower(string input)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                int alphabetIndex = alphabet.IndexOf(char.ToLower(c));
                if (alphabetIndex == alphabet.Length - 1)
                    result += alphabet[0];
                else
                    result += alphabet[(alphabetIndex + 1)];
            }

            return result;
        }
    }
}