using Banapuchin.Classes;
using Il2CppLocomotion;
using UnityEngine;

namespace Banapuchin.Mods.Movement
{
    public class Airplane : ModBase
    {
        public override string Text => "Airplane";

        public override void FixedUpdate()
        {
            Vector3 uno =
                Player.Instance.LeftHand.transform.InverseTransformPoint(Player.Instance.RightHand.transform.position);

            if (uno.z < 0f &&
                Vector3.Dot(Player.Instance.RightHand.transform.forward, Player.Instance.LeftHand.transform.forward) >
                0.4f &&
                Vector3.Dot(Player.Instance.RightHand.transform.right, Player.Instance.LeftHand.transform.right) < 0.5f)
            {
                Vector3 direction = Player.Instance.playerCam.gameObject.transform.forward;
                Player.Instance.playerRigidbody.velocity =
                    Vector3.Lerp(Player.Instance.playerRigidbody.velocity, direction * 10f, 0.5f);
            }
        }
    }
}