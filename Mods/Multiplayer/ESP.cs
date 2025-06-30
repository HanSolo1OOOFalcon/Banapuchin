using Banapuchin.Classes;
using Banapuchin.Patches;
using Il2Cpp;
using UnityEngine;

namespace Banapuchin.Mods.Multiplayer
{
    public class ESP : ModBase
    {
        public override string Text => "ESP";
        
        public override void OnEnable()
        {
            base.OnEnable();
            FusionPlayerSpawnedPatch.ShouldESP = true;
            foreach (var thing in FusionHub.Instance.SpawnedPlayers)
            {
                FusionPlayer player = thing.Item1;
                if (player.IsLocalPlayer)
                    continue;

                player.skin.material.color = player.__Color;
                player.skin.material.shader = Shader.Find("GUI/Text Shader");
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            FusionPlayerSpawnedPatch.ShouldESP = false;
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