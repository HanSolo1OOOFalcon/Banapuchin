using Banapuchin.Classes;
using Caputilla.Utils;
using Locomotion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class OtherFly : ModBase
    {
        public override string Text => "Other Fly";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Fly), typeof(WeirdFly) };

        public override void FixedUpdate()
        {
            if (ControllerInputManager.Instance.leftPrimary)
            {
                Player.Instance.transform.position += Player.Instance.playerCam.transform.forward * 0.5f;
                Player.Instance.playerRigidbody.AddForce(-Physics.gravity * Player.Instance.playerRigidbody.mass * Player.Instance.scale);
            }
        }
    }
}