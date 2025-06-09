using Caputilla.Utils;
using Banapuchin.Classes;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class Fly : ModBase
    {
        public override string Text => "Fly";

        public override void FixedUpdate()
        {
            Vector3 inputs = new Vector3(ControllerInputManager.Instance.leftStickAxis.x, ControllerInputManager.Instance.leftStickAxis.y, ControllerInputManager.Instance.rightStickAxis.y);
            Vector3 forward = Player.Instance.playerCam.transform.forward;
            Vector3 right = Player.Instance.playerCam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            Vector3 velo = inputs.x * right + inputs.y * forward + inputs.z * Vector3.up;
            velo *= 15f;
            Player.Instance.playerRigidbody.velocity = velo;
        }
    }
}
