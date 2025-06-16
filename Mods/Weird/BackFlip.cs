using Banapuchin.Classes;
using Banapuchin.Libraries;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Locomotion;
using System.Collections;
using UnityEngine;

namespace Banapuchin.Mods.Weird
{
    public class BackFlip : ModBase
    {
        public override string Text => "Backflip";

        static bool wasPressed;

        public override void FixedUpdate()
        {
            bool rightPrimary = ControllerInput.instance.GetInput(ControllerInput.InputType.rightPrimaryButton);

            if (rightPrimary)
            {
                if (!wasPressed)
                {
                    wasPressed = true;
                    CoroutineManager.instance.StartCoroutine(BackFlipCoroutine().WrapToIl2Cpp());
                }
            }
            else
            {
                wasPressed = false;
            }
        }

        private IEnumerator BackFlipCoroutine()
        {
            int i = 0;
            while (i < 360)
            {
                Player.Instance.transform.Rotate(-Player.Instance.transform.forward, 1, Space.Self);
                i++;
                yield return new WaitForSeconds(0.25f/360f);
            }
        }
    }
}
