using Banapuchin.Classes;
using Locomotion;
using Banapuchin.Libraries;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class OtherFly : ModBase
    {
        public override string Text => "Other Fly";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Fly), typeof(WeirdFly) };

        public override void Update()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.leftPrimaryButton))
            {
                Player.Instance.transform.position += Player.Instance.playerCam.transform.forward * 0.5f;
                Player.Instance.playerRigidbody.velocity = Vector3.zero;
            }
        }
    }
}