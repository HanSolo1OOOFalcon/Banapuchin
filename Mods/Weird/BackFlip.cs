using Banapuchin.Classes;
using Caputilla.Utils;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Mods.Weird
{
    public class BackFlip : ModBase
    {
        public override string Text => "Backflip";

        static bool wasPressed;
        public override void FixedUpdate()
        {
            if (ControllerInputManager.Instance.rightPrimary)
            {
                if (!wasPressed)
                {
                    wasPressed = true;
                    for (int i = 0; i < 360; i++)
                    {
                        Player.Instance.transform.Rotate(Vector3.right, 1, Space.Self);
                    }
                }
            }
            else
            {
                wasPressed = false;
            }
        }
    }
}
