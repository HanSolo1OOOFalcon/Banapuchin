using Banapuchin.Libraries;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class BaseButtonManager : BetterMonoBehaviour
    {
        private static float _buttonCooldown;
        public bool isAllowed;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (Time.time > _buttonCooldown)
            {
                if (other.gameObject == PublicThingsHerePlease.BallR)
                {
                    _buttonCooldown = Time.time + 0.2f;
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, false);
                    isAllowed = true;
                }

                if (other.gameObject == PublicThingsHerePlease.BallL)
                {
                    _buttonCooldown = Time.time + 0.2f;
                    HapticLibrary.instance.SendHaptics(0.5f, 0.2f, true);
                    isAllowed = true;
                }
            }
        }
    }
}