using Caputilla.Utils;
using Locomotion;
using Banapuchin.Classes;
using Banapuchin.Libraries;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class IronCapu : ModBase
    {
        public override string Text => "Iron Man";

        static bool l, r;

        public override void OnDisable()
        {
            base.OnDisable();
            HapticLibrary.instance.StopHaptics(false);
            HapticLibrary.instance.StopHaptics(true);
        }

        public override void FixedUpdate()
        {
            if (ControllerInputManager.Instance.rightGrip)
            {
                Player.Instance.playerRigidbody.AddForce(12f * Player.Instance.RightHand.transform.right, ForceMode.Acceleration);

                if (!r)
                {
                    r = true;
                    HapticLibrary.instance.StartHaptics(0.7f, false);
                }
            }
            else if (r)
            {
                r = false;
                HapticLibrary.instance.StopHaptics(false);
            }

            if (ControllerInputManager.Instance.leftGrip)
            {
                Player.Instance.playerRigidbody.AddForce(12f * -Player.Instance.LeftHand.transform.right, ForceMode.Acceleration);

                if (!l)
                {
                    l = true;
                    HapticLibrary.instance.StartHaptics(0.7f, true);
                }
            }
            else if (l)
            {
                l = false;
                HapticLibrary.instance.StopHaptics(true);
            }
        }
    }
}
