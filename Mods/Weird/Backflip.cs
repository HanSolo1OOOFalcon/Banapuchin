using Banapuchin.Classes;
using Banapuchin.Libraries;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Locomotion;
using System.Collections;
using UnityEngine;

namespace Banapuchin.Mods.Weird
{
    public class Backflip : ModBase
    {
        public override string Text => "Backflip";

        private static Vector3 cachedDir;

        public override void Update()
        {
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.leftSecondaryButton))
            {
                cachedDir = Player.Instance.playerCam.gameObject.gameObject.transform.right;
                CoroutineManager.instance.StartCoroutine(doAFlip().WrapToIl2Cpp());
            }
        }

        private IEnumerator doAFlip()
        {
            float rotatedAmount = 0f;
            float rotationSpeed = 360f;

            while (rotatedAmount < 360f)
            {
                float rotationStep = rotationSpeed * Time.deltaTime;
                Player.Instance.transform.Rotate(cachedDir * rotationStep);
                rotatedAmount += rotationStep;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
