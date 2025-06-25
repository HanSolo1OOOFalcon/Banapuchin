using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Banapuchin.Libraries
{
    public class ControllerInput : MonoBehaviour
    {
        public static ControllerInput instance;

        public enum InputType
        {
            leftGrip, rightGrip,
            leftTrigger, rightTrigger,
            leftPrimaryButton, rightPrimaryButton,
            leftSecondaryButton, rightSecondaryButton,
        }

        public enum StickTypes
        {
            leftStickAxis, rightStickAxis,
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
                case InputType.leftGrip:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip) && leftGrip;
                case InputType.rightGrip:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip) && rightGrip;
                case InputType.leftTrigger:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTrigger) && leftTrigger;
                case InputType.rightTrigger:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTrigger) && rightTrigger;
                case InputType.leftPrimaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimaryButton) && leftPrimaryButton;
                case InputType.rightPrimaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimaryButton) && rightPrimaryButton;
                case InputType.leftSecondaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool leftSecondaryButton) && leftSecondaryButton;
                case InputType.rightSecondaryButton:
                    return InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightSecondaryButton) && rightSecondaryButton;
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
                case StickTypes.leftStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftAxis);
                    return leftAxis;
                case StickTypes.rightStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightAxis);
                    return rightAxis;
            }
            return Vector2.zero;
        }
    }
}