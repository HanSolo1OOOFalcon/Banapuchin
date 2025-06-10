using Banapuchin.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Multiplayer
{
    public class Tracers : ModBase
    {
        public override string Text => "Tracers";

        static List<LineRenderer> tracers = new List<LineRenderer>();

        public override void Update()
        {
            foreach (var thing in FusionHub.Instance.SpawnedPlayers)
            {
                FusionPlayer player = thing.Item1;
                if (player.IsLocalPlayer) continue;
                var head = player.transform.Find("Head").gameObject;
                LineRenderer tracer = head.GetComponent<LineRenderer>();

                if (tracer == null)
                {
                    tracer = head.AddComponent<LineRenderer>();
                    tracer.startWidth = 0.005f;
                    tracer.endWidth = 0.005f;
                    tracer.material.shader = Shader.Find("GUI/Text Shader");
                    tracer.material.color = Color.white * 0.75f;
                    tracers.Add(tracer);
                }

                tracer.SetPosition(0, Locomotion.Player.Instance.RightHand.transform.position);
                tracer.SetPosition(1, head.transform.position);
            }
        }

        public override void OnDisable()
        {
            foreach (LineRenderer tracer in tracers)
            {
                if (tracer != null)
                {
                    GameObject.Destroy(tracer);
                }
            }
            tracers.Clear();
        }
    }
}
