using Banapuchin.Classes;
using UnityEngine;
using UnityEngine.XR;

namespace Banapuchin.Libraries
{
    public class ControllerInput : BetterMonoBehaviour
    {
        public static ControllerInput instance;

        public enum InputType
        {
            LeftGrip, RightGrip,
            LeftTrigger, RightTrigger,
            LeftPrimaryButton, RightPrimaryButton,
            LeftSecondaryButton, RightSecondaryButton,
        }

        public enum StickTypes
        {
            LeftStickAxis, RightStickAxis,
        }

        private readonly Dictionary<InputType, bool> previousStates = new Dictionary<InputType, bool>();
        private readonly Dictionary<InputType, bool> currentStates = new Dictionary<InputType, bool>();

        void Start()
        {
            instance = this;

            foreach (InputType input in System.Enum.GetValues(typeof(InputType)))
            {
                previousStates[input] = false;
                currentStates[input] = false;
            }
        }

        void Update()
        {
            foreach (InputType input in System.Enum.GetValues(typeof(InputType)))
            {
                previousStates[input] = currentStates[input];
                currentStates[input] = GetInputRaw(input);
            }
        }

        private bool GetInputRaw(InputType wantedInput)
        {
            switch (wantedInput)
            {
                case InputType.LeftGrip:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                        .TryGetFeatureValue(CommonUsages.gripButton, out bool LeftGrip) && LeftGrip;
                case InputType.RightGrip:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                        .TryGetFeatureValue(CommonUsages.gripButton, out bool RightGrip) && RightGrip;
                case InputType.LeftTrigger:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                        .TryGetFeatureValue(CommonUsages.triggerButton, out bool LeftTrigger) && LeftTrigger;
                case InputType.RightTrigger:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                        .TryGetFeatureValue(CommonUsages.triggerButton, out bool RightTrigger) && RightTrigger;
                case InputType.LeftPrimaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                               .TryGetFeatureValue(CommonUsages.primaryButton, out bool LeftPrimaryButton) &&
                           LeftPrimaryButton;
                case InputType.RightPrimaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                               .TryGetFeatureValue(CommonUsages.primaryButton, out bool RightPrimaryButton) &&
                           RightPrimaryButton;
                case InputType.LeftSecondaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                               .TryGetFeatureValue(CommonUsages.secondaryButton, out bool LeftSecondaryButton) &&
                           LeftSecondaryButton;
                case InputType.RightSecondaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                               .TryGetFeatureValue(CommonUsages.secondaryButton, out bool RightSecondaryButton) &&
                           RightSecondaryButton;
            }

            return false;
        }

        public bool GetInput(InputType wantedInput)
        {
            return currentStates[wantedInput];
        }

        public bool GetInputDown(InputType wantedInput)
        {
            return currentStates[wantedInput] && !previousStates[wantedInput];
        }

        public bool GetInputUp(InputType wantedInput)
        {
            return !currentStates[wantedInput] && previousStates[wantedInput];
        }

        public Vector2 GetAxis(StickTypes wantedAxis)
        {
            switch (wantedAxis)
            {
                case StickTypes.LeftStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                        .TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 LeftAxis);
                    return LeftAxis;
                case StickTypes.RightStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                        .TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 RightAxis);
                    return RightAxis;
            }

            return Vector2.zero;
        }
    }
}