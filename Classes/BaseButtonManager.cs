using BepInEx.Logging;
using Locomotion;
using Banapuchin.Libraries;
using Banapuchin;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class BaseButtonManager : BetterMonoBehaviour
    {
        public static float buttonCooldown;
        public bool isAllowed = false;

        public virtual void OnTriggerEnter(Collider other)
        {
            if (Time.time > buttonCooldown)
            {
                if (other.gameObject == PublicThingsHerePlease.rBall)
                {
                    buttonCooldown = Time.time + 0.2f;
                    Player.Instance.clickSound(false, 13);
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, false);
                    isAllowed = true;
                }

                if (other.gameObject == PublicThingsHerePlease.lBall)
                {
                    buttonCooldown = Time.time + 0.2f;
                    Player.Instance.clickSound(true, 13);
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, true);
                    isAllowed = true;
                }
            }
        }
    }
}