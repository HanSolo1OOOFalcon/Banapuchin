using BepInEx.Unity.IL2CPP.Utils.Collections;
using System.Collections;
using UnityEngine;

namespace Banapuchin.Libraries
{
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager instance;

        public Coroutine RunCoroutine(IEnumerator routine)
        {
            if (!FusionHub.currentQueue.ToLower().Contains("modded")) Application.Quit();
            return StartCoroutine(routine.WrapToIl2Cpp());
        }

        void Start() => instance = this;
    }
}