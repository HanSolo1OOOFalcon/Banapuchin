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
            base.OnEnable();
            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("Banana");
            checkpointObj = GameObject.Instantiate(foo);
            checkpointObj.name = "CheckpointBanana";
            checkpointObj.transform.localScale = Vector3.one * 8f;
            PublicThingsHerePlease.FixShaders(checkpointObj);
            checkpointObj.SetActive(false);
        }

        public override void OnDisable()
        {
            base.OnDisable();
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
                checkpointObj.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
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
