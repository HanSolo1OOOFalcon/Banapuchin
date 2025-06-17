using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Patches;
using Banapuchin.Libraries;
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

        public override void FixedUpdate()
        {
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.rightTrigger))
            {
                checkpointObj.SetActive(true);
                checkpointObj.transform.position = Player.Instance.RightHand.transform.position;
                checkpointObj.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }

            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.leftTrigger) && checkpointObj.activeSelf)
            {
                TeleportPatch.TeleportPlayer(checkpointObj.transform.position, true);
            }
        }
    }
}