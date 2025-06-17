using Banapuchin.Classes;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Multiplayer
{
    public class Tracers : ModBase
    {
        public override string Text => "Tracers";

        static List<LineRenderer> tracers = new List<LineRenderer>();

        private int pointsPerTracer = 100;
        private float spiralTurns = 3f;
        private float rotationSpeed = 4f;
        private float pulseSpeed = 5f;
        private float pulseAmount = 0.04f;
        private float baseRadius = 0.05f;

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

                    tracer.positionCount = pointsPerTracer;
                    tracer.startWidth = 0.005f;
                    tracer.endWidth = 0.005f;

                    tracer.material.shader = Shader.Find("GUI/Text Shader");
                    tracer.material.color = player.__Color;

                    tracers.Add(tracer);
                }

                float currentRadius = baseRadius + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

                Vector3 start = Locomotion.Player.Instance.RightHand.transform.position;
                Vector3 end = head.transform.position;

                Vector3 direction = (end - start).normalized;
                float distance = Vector3.Distance(start, end);

                Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;

                if (right == Vector3.zero)
                    right = Vector3.Cross(direction, Vector3.forward).normalized;

                float angleOffset = Time.time * rotationSpeed;

                for (int i = 0; i < pointsPerTracer; i++)
                {
                    float t = (float)i / (pointsPerTracer - 1);
                    Vector3 pointOnLine = Vector3.Lerp(start, end, t);

                    float angle = t * spiralTurns * Mathf.PI * 2 + angleOffset;

                    Vector3 offset = (Quaternion.AngleAxis(angle * Mathf.Rad2Deg, direction) * right) * currentRadius;

                    tracer.SetPosition(i, pointOnLine + offset);
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
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
