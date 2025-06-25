using Banapuchin.Classes;
using UnityEngine;
using Locomotion;
using System;
using System.Collections.Generic;
using Banapuchin.Mods.Weird;

namespace Banapuchin.Mods.Movement
{
    public class ZeroGravity : ModBase
    {
        public override string Text => "Zero Gravity";
        public override List<Type> Incompatibilities => new List<Type> { typeof(UpsideDown) };

        public override void OnEnable()
        {
            base.OnEnable();
            PublicThingsHerePlease.Menu.GetComponent<Rigidbody>().useGravity = false;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PublicThingsHerePlease.Menu.GetComponent<Rigidbody>().useGravity = true;
        }

        public override void FixedUpdate() =>
            Player.Instance.playerRigidbody.AddForce(-Physics.gravity * Player.Instance.playerRigidbody.mass);
    }
}