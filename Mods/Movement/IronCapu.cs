using Locomotion;
using Banapuchin.Classes;
using Banapuchin.Libraries;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Banapuchin.Mods.Movement
{
    public class IronCapu : ModBase
    {
        public override string Text => "Iron Capu";
        public override List<Type> Incompatibilities => new List<Type> { typeof(WeirdFly) };

        static bool l, r;
        static ParticleSystem leftParticle, rightParticle;
        static AudioSource leftAudio, rightAudio;

        public override void OnEnable()
        {
            base.OnEnable();
            GameObject particle = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("FireParticles");
            particle.GetComponent<ParticleSystemRenderer>().material.shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");

            leftParticle = GameObject.Instantiate(particle.GetComponent<ParticleSystem>());
            leftParticle.transform.SetParent(Player.Instance.LeftHand.transform);
            leftParticle.transform.localPosition = Vector3.zero;
            leftParticle.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            leftParticle.Stop();

            leftAudio = leftParticle.gameObject.AddComponent<AudioSource>();
            leftAudio.clip = PublicThingsHerePlease.bundle.LoadAsset<AudioClip>("FlameSound");
            leftAudio.loop = true;
            leftAudio.spatialBlend = 1f;
            leftAudio.playOnAwake = false;

            rightParticle = GameObject.Instantiate(particle.GetComponent<ParticleSystem>());
            rightParticle.transform.SetParent(Player.Instance.RightHand.transform);
            rightParticle.transform.localPosition = Vector3.zero;
            rightParticle.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
            rightParticle.Stop();

            rightAudio = rightParticle.gameObject.AddComponent<AudioSource>();
            rightAudio.clip = PublicThingsHerePlease.bundle.LoadAsset<AudioClip>("FlameSound");
            rightAudio.loop = true;
            rightAudio.spatialBlend = 1f;
            rightAudio.playOnAwake = false;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            HapticLibrary.instance.StopHaptics(false);
            HapticLibrary.instance.StopHaptics(true);
            GameObject.Destroy(leftParticle.gameObject);
            GameObject.Destroy(rightParticle.gameObject);
        }

        public override void Update()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.rightGrip))
            {
                Player.Instance.playerRigidbody.AddForce(12f * Player.Instance.RightHand.transform.right, ForceMode.Acceleration);

                if (!r)
                {
                    r = true;
                    HapticLibrary.instance.StartHaptics(0.7f, false);
                    rightParticle.transform.SetParent(Player.Instance.RightHand.transform, false);
                    rightParticle.transform.localPosition = Vector3.zero;
                    rightParticle.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    rightParticle.Play();
                    rightAudio.Play();
                }
            }
            else if (r)
            {
                r = false;
                HapticLibrary.instance.StopHaptics(false);
                rightParticle.transform.SetParent(null, true);
                rightParticle.Stop();
                rightAudio.Stop();
            }

            if (ControllerInput.instance.GetInput(ControllerInput.InputType.leftGrip))
            {
                Player.Instance.playerRigidbody.AddForce(12f * -Player.Instance.LeftHand.transform.right, ForceMode.Acceleration);

                if (!l)
                {
                    l = true;
                    HapticLibrary.instance.StartHaptics(0.7f, true);
                    leftParticle.transform.SetParent(Player.Instance.LeftHand.transform, false);
                    leftParticle.transform.localPosition = Vector3.zero;
                    leftParticle.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    leftParticle.Play();
                    leftAudio.Play();
                }
            }
            else if (l)
            {
                l = false;
                HapticLibrary.instance.StopHaptics(true);
                leftParticle.transform.SetParent(null, true);
                leftParticle.Stop();
                leftAudio.Stop();
            }
        }
    }
}