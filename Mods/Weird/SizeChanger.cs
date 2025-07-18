using Banapuchin.Classes;
using Banapuchin.Patches;
using Banapuchin.Libraries;
using Il2CppLocomotion;
using UnityEngine;

namespace Banapuchin.Mods.Weird
{
    public class SizeChanger : ModBase
    {
        public override string Text => "Size Changer";

        public override void Update()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.RightTrigger))
            {
                Player.Instance.scale += Player.Instance.scale * 0.01f;
            }
            else if (ControllerInput.instance.GetInput(ControllerInput.InputType.LeftTrigger))
            {
                Player.Instance.scale -= Player.Instance.scale * 0.01f;
                if (Player.Instance.scale < 0.01f)
                {
                    Player.Instance.scale = 0.01f;
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            float toMultiply = Player.Instance.scale;
            Player.Instance.scale = 1f;
            TeleportPatch.TeleportPlayer(Player.Instance.transform.position + Vector3.up * toMultiply, true);
        }
    }
}