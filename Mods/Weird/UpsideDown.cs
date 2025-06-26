using Banapuchin.Classes;
using UnityEngine;
using Il2CppLocomotion;
using Banapuchin.Mods.Movement;

namespace Banapuchin.Mods.Weird
{
    public class UpsideDown : ModBase
    {
        public override string Text => "Upside Down";
        public override List<Type> Incompatibilities => new List<Type> { typeof(ZeroGravity) };

        public override void OnEnable()
        {
            base.OnEnable();
            Player.Instance.transform.position += Vector3.up * Player.Instance.scale * 2f;
            Player.Instance.transform.Rotate(Vector3.right, 180, Space.Self);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Player.Instance.transform.position += Vector3.down * Player.Instance.scale * 2f;
            Player.Instance.transform.Rotate(Vector3.right, -180, Space.Self);
        }

        public override void FixedUpdate()
        {
            Player.Instance.playerRigidbody.AddForce((-Physics.gravity * 2) * Player.Instance.playerRigidbody.mass);
            if (!PublicThingsHerePlease.Menu.GetComponent<Rigidbody>().isKinematic)
                PublicThingsHerePlease.Menu.GetComponent<Rigidbody>().AddForce((-Physics.gravity * 2) * PublicThingsHerePlease.Menu.GetComponent<Rigidbody>().mass);
        }
    }
}