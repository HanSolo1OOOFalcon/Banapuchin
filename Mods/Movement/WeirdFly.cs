using Banapuchin.Classes;
using Il2CppLocomotion;
using Banapuchin.Libraries;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class WeirdFly : ModBase
    {
        public override string Text => "Weird Fly";
        public override List<Type> Incompatibilities =>
            new List<Type> { typeof(Fly), typeof(OtherFly), typeof(IronCapu) };

        public override void Update()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.LeftGrip) &&
                ControllerInput.instance.GetInput(ControllerInput.InputType.RightGrip))
            {
                float magnitude =
                    (Player.Instance.LeftHand.transform.position - Player.Instance.RightHand.transform.position)
                    .magnitude;
                Vector3 direction = Player.Instance.playerCam.gameObject.transform.forward;
                Player.Instance.transform.position += direction * magnitude;
                Player.Instance.playerRigidbody.velocity = Vector3.zero;
            }
        }
    }
}