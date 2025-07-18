using Banapuchin.Classes;
using UnityEngine;
using UnityEngine.XR;

namespace Banapuchin.Libraries
{
    public class HapticLibrary : BetterMonoBehaviour
    {
        public static HapticLibrary instance;
        private bool shouldSendHapticsL, shouldSendHapticsR;
        private float amplitudeL, amplitudeR;

        void Start() => instance = this;

        void FixedUpdate()
        {
            if (shouldSendHapticsL)
                SendHaptics(amplitudeL, Time.fixedDeltaTime,
                    true); // uhh summa lumma domma lumma you assumin im a human what i got to do to get it through to you im super human innovative and im made of rubber so that anything you say is ricocheting of of me and itll glue to you im devestating more than ever demonstrating how to give a motherfucking audience a feeling like its levitating never fading and i know the haters are forever waiting for the day that they can say i fell off they be celeberating cuz i know the way to get them motivated i make elevating music you make elevator music oh hes to mainstream well thats what they do they get jealous they confuse it its not hip hop its pop cuz i found a hella way to fuse it

            if (shouldSendHapticsR)
                SendHaptics(amplitudeR, Time.fixedDeltaTime, false);
        }

        public void StartHaptics(float amplitude, bool isLeft)
        {
            if (isLeft)
            {
                shouldSendHapticsL = true;
                amplitudeL = amplitude;
            }
            else
            {
                shouldSendHapticsR = true;
                amplitudeR = amplitude;
            }
        }

        public void StopHaptics(bool isLeft)
        {
            if (isLeft)
            {
                shouldSendHapticsL = false;
                amplitudeL = 0f;
            }
            else
            {
                shouldSendHapticsR = false;
                amplitudeR = 0f;
            }
        }

        public void SendHaptics(float amplitude, float duration, bool isLeft)
        {
            InputDevice device = isLeft
                ? InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                : InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            uint channel = 0u;
            device.SendHapticImpulse(channel, amplitude, duration);
        }
    }
}