using Banapuchin.Classes;
using System.Collections.Generic;
using Il2CppSystem.IO;
using System.Reflection;
using UnityEngine;
using Banapuchin.Main;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

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
    }
}