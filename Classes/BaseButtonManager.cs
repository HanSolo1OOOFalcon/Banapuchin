using Banapuchin.Libraries;
using Banapuchin.Main;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class BaseButtonManager : BetterMonoBehaviour
    {
        public static float buttonCooldown;
        public bool isAllowed = false;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!FusionHub.currentQueue.ToLower().Contains("modded")) Application.Quit();
            if (Time.time > buttonCooldown)
            {
                if (other.gameObject == PublicThingsHerePlease.rBall)
                {
                    buttonCooldown = Time.time + 0.2f;
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, false);
                    isAllowed = true;
                }

                if (other.gameObject == PublicThingsHerePlease.lBall)
                {
                    buttonCooldown = Time.time + 0.2f;
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, true);
                    isAllowed = true;
                }
            }
        }
    }
}