using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Patches;
using Banapuchin.Libraries;
using Il2CppLocomotion;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class Checkpoints : ModBase
    {
        public override string Text => "Checkpoints";

        private static GameObject _checkpointObj;

        public override void OnEnable()
        {
            base.OnEnable();
            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("Banana");
            _checkpointObj = UnityEngine.Object.Instantiate(foo);
            _checkpointObj.name = "CheckpointBanana";
            _checkpointObj.transform.localScale = Vector3.one * 8f;
            PublicThingsHerePlease.FixShaders(_checkpointObj);
            _checkpointObj.SetActive(false);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_checkpointObj != null)
            {
                _checkpointObj.Obliterate(out _checkpointObj);
            }
        }

        public override void Update()
        {
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.rightTrigger))
            {
                _checkpointObj.SetActive(true);
                _checkpointObj.transform.position = Player.Instance.RightHand.transform.position;
                _checkpointObj.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            }

            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.leftTrigger) &&
                _checkpointObj.activeSelf)
            {
                TeleportPatch.TeleportPlayer(_checkpointObj.transform.position, true);
            }
        }
    }
}