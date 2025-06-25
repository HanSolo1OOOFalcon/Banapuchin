using System.Collections;
using Banapuchin.Classes;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace Banapuchin.Libraries
{
    public class CoroutineManager : BetterMonoBehaviour
    {
        public static CoroutineManager instance;

        public void RunCoroutine(IEnumerator routine)
        {
            if (!FusionHub.currentQueue.ToLower().Contains("modded")) Application.Quit();
            MelonCoroutines.Start(routine);
        }

        void Start() => instance = this;
    }
}