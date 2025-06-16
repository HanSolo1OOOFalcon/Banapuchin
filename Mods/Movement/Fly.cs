using Caputilla.Utils;
using Banapuchin.Classes;
using Locomotion;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Banapuchin.Mods.Movement
{
    public class Fly : ModBase
    {
        public override string Text => "Fly";
        public override List<Type> Incompatibilities => new List<Type> { typeof(OtherFly), typeof(WeirdFly) };

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
            Player.Instance.playerRigidbody.AddForce(-Physics.gravity * Player.Instance.playerRigidbody.mass * Player.Instance.scale);
        }
    }
}
