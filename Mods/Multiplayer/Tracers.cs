using Banapuchin.Classes;
using Il2Cpp;
using UnityEngine;
using Il2CppLocomotion;

namespace Banapuchin.Mods.Multiplayer
{
    public class Tracers : ModBase
    {
        public override string Text => "Tracers";

        private static List<LineRenderer> _tracers = new List<LineRenderer>();

        private const int PointsPerTracer = 100;

        private const float SpiralTurns = 3f,
            RotationSpeed = 4f,
            PulseSpeed = 5f,
            PulseAmount = 0.04f,
            BaseRadius = 0.05f;

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

                    tracer.positionCount = PointsPerTracer;
                    tracer.startWidth = 0.005f;
                    tracer.endWidth = 0.005f;

                    tracer.material.shader = Shader.Find("GUI/Text Shader");
                    tracer.material.color = player.__Color;

                    _tracers.Add(tracer);
                }

                float currentRadius = BaseRadius + Mathf.Sin(Time.time * PulseSpeed) * PulseAmount;

                Vector3 start = Player.Instance.RightHand.transform.position;
                Vector3 end = head.transform.position;

                Vector3 direction = (end - start).normalized;

                Vector3 right = Vector3.Cross(direction, Vector3.up).normalized;

                if (right == Vector3.zero)
                    right = Vector3.Cross(direction, Vector3.forward).normalized;

                float angleOffset = Time.time * RotationSpeed;

                for (int i = 0; i < PointsPerTracer; i++)
                {
                    float t = (float)i / (PointsPerTracer - 1);
                    Vector3 pointOnLine = Vector3.Lerp(start, end, t);

                    float angle = t * SpiralTurns * Mathf.PI * 2 + angleOffset;

                    Vector3 offset = (Quaternion.AngleAxis(angle * Mathf.Rad2Deg, direction) * right) * currentRadius;

                    tracer.SetPosition(i, pointOnLine + offset);
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            foreach (LineRenderer tracer in _tracers)
            {
                if (tracer != null)
                {
                    UnityEngine.Object.Destroy(tracer);
                }
            }

            _tracers.Clear();
        }
    }
}