using Banapuchin.Classes;
using UnityEngine;

namespace Banapuchin.Mods.Multiplayer
{
    public class ESP : ModBase
    {
        public override string Text => "ESP";
        public override void OnEnable()
        {
            base.OnEnable();
            foreach (var thing in FusionHub.Instance.SpawnedPlayers)
            {
                FusionPlayer player = thing.Item1;
                if (player.IsLocalPlayer)
                    continue;

                player.skin.material.color = new Color(player.currentColor.x, player.currentColor.y, player.currentColor.z, 1f);
                player.skin.material.shader = Shader.Find("GUI/Text Shader");
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            foreach (var thing in FusionHub.Instance.SpawnedPlayers)
            {
                FusionPlayer player = thing.Item1;
                if (player.IsLocalPlayer)
                    continue;

                player.skin.material.shader = Shader.Find("Shader Graphs/ShadedPiss");
            }
        }
    }
}
