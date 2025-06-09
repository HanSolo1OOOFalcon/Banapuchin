using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Patches;
using Caputilla.Utils;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class Checkpoints : ModBase
    {
        public override string Text => "Checkpoints";

        static GameObject checkpointObj;

        public override void OnEnable()
        {
            PublicThingsHerePlease.CreateBanana(out checkpointObj);
            checkpointObj.transform.localScale = Vector3.one * 0.5f;
            checkpointObj.SetActive(false);
        }

        public override void OnDisable()
        {
            if (checkpointObj != null)
            {
                checkpointObj.Obliterate(out checkpointObj);
            }
        }

        static bool wasPressed1, wasPressed2;
        public override void FixedUpdate()
        {
            if (ControllerInputManager.Instance.rightTrigger && !wasPressed1)
            {
                checkpointObj.SetActive(true);
                checkpointObj.transform.position = Player.Instance.RightHand.transform.position;
                checkpointObj.transform.rotation = Quaternion.Euler(0f, Player.Instance.RightHand.transform.rotation.eulerAngles.y, 90f);
            }

            if (ControllerInputManager.Instance.leftTrigger && !wasPressed2 && checkpointObj.activeSelf)
            {
                TeleportPatch.TeleportPlayer(checkpointObj.transform.position, true);
            }

            wasPressed1 = ControllerInputManager.Instance.rightTrigger;
            wasPressed2 = ControllerInputManager.Instance.leftTrigger;
        }
    }
}
