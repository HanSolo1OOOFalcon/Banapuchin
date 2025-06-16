using Banapuchin.Classes;
using Caputilla.Utils;
using Locomotion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class WeirdFly : ModBase
    {
        public override string Text => "Weird Fly";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Fly), typeof(OtherFly), typeof(IronCapu) };

        public override void FixedUpdate()
        {
            if (ControllerInputManager.Instance.rightGrip && ControllerInputManager.Instance.leftGrip)
            {
                float magnitude = (Player.Instance.LeftHand.transform.position - Player.Instance.RightHand.transform.position).magnitude;
                Vector3 direction = Player.Instance.playerCam.gameObject.transform.forward;
                Player.Instance.transform.position += direction * magnitude;
                Player.Instance.playerRigidbody.AddForce(-Physics.gravity * Player.Instance.playerRigidbody.mass * Player.Instance.scale);
            }
        }
    }
}
