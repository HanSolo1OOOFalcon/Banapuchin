using Locomotion;
using Banapuchin.Classes;
using Banapuchin.Libraries;
using UnityEngine;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Banapuchin.Mods.Movement
{
    public class IronCapu : ModBase
    {
        public override string Text => "Iron Capu";
        public override List<Type> Incompatibilities => new List<Type> { typeof(WeirdFly) };

        private static bool _left, _right;
        private static ParticleSystem _leftParticle, _rightParticle;
        private static AudioSource _leftAudio, _rightAudio;

        public override void OnEnable()
        {
            base.OnEnable();
            GameObject particle = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("FireParticles");
            particle.GetComponent<ParticleSystemRenderer>().material.shader =
                Shader.Find("Universal Render Pipeline/Particles/Unlit");
            AudioClip fireClip = PublicThingsHerePlease.bundle.LoadAsset<AudioClip>("FlameSound");

            _leftParticle = Object.Instantiate(original: particle.GetComponent<ParticleSystem>(),
                parent: Player.Instance.LeftHand.transform);
            _leftParticle.transform.localPosition = Vector3.zero;
            _leftParticle.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            _leftParticle.main.simulationSpace = ParticleSystemSimulationSpace.World;
            _leftParticle.Stop();

            _leftAudio = _leftParticle.gameObject.AddComponent<AudioSource>();
            _leftAudio.clip = fireClip;
            _leftAudio.loop = true;
            _leftAudio.spatialBlend = 1f;
            _leftAudio.playOnAwake = false;

            _rightParticle = Object.Instantiate(original: particle.GetComponent<ParticleSystem>(),
                parent: Player.Instance.RightHand.transform);
            _rightParticle.transform.localPosition = Vector3.zero;
            _rightParticle.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
            _rightParticle.main.simulationSpace = ParticleSystemSimulationSpace.World;
            _rightParticle.Stop();

            _rightAudio = _rightParticle.gameObject.AddComponent<AudioSource>();
            _rightAudio.clip = fireClip;
            _rightAudio.loop = true;
            _rightAudio.spatialBlend = 1f;
            _rightAudio.playOnAwake = false;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            HapticLibrary.instance.StopHaptics(false);
            HapticLibrary.instance.StopHaptics(true);
            Object.Destroy(_leftParticle.gameObject);
            Object.Destroy(_rightParticle.gameObject);
        }

        public override void Update()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.rightGrip))
            {
                Player.Instance.playerRigidbody.AddForce(12f * Player.Instance.RightHand.transform.right,
                    ForceMode.Acceleration);

                if (!_right)
                {
                    _right = true;
                    HapticLibrary.instance.StartHaptics(0.7f, false);
                    _rightParticle.Play();
                    _rightAudio.Play();
                }
            }
            else if (_right)
            {
                _right = false;
                HapticLibrary.instance.StopHaptics(false);
                _rightParticle.Stop();
                _rightAudio.Stop();
            }

            if (ControllerInput.instance.GetInput(ControllerInput.InputType.leftGrip))
            {
                Player.Instance.playerRigidbody.AddForce(12f * -Player.Instance.LeftHand.transform.right,
                    ForceMode.Acceleration);

                if (!_left)
                {
                    _left = true;
                    HapticLibrary.instance.StartHaptics(0.7f, true);
                    _leftParticle.Play();
                    _leftAudio.Play();
                }
            }
            else if (_left)
            {
                _left = false;
                HapticLibrary.instance.StopHaptics(true);
                _leftParticle.Stop();
                _leftAudio.Stop();
            }
        }
    }
}