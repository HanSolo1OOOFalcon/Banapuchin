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

        void Start()
        {
            instance = this;
        }

        public bool GetInput(InputType wantedInput)
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

        public float GetAxis(StickTypes wantedAxis)
        {
            switch (wantedAxis)
            {
                case StickTypes.leftStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftAxis);
                    return leftAxis.x;
                case StickTypes.rightStickAxis:
                    InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightAxis);
                    return rightAxis.x;
            }
            return 0f;
        }
    }
}
