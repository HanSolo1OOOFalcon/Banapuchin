using Banapuchin.Libraries;
using Locomotion;
using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Classes
{
    public class HoldableObject : BetterMonoBehaviour
    {
        public float grabDistance = 0.2f;
        private Rigidbody rb;
        private bool beingHeldR, beingHeldL;
        private Vector3 lastPosition;
        private Vector3 velocity;

        void Start()
        {
            rb = transform.gameObject.GetComponent<Rigidbody>();
            if (!FusionHub.currentQueue.ToLower().Contains(GetStringToLower("lNcCDc"))) Application.Quit();
        }

        void Update()
        {
            grabDistance = 0.2f * Player.Instance.scale;
            if (!beingHeldR && !beingHeldL)
            {
                TryGrab();
            }
            else
            {
                velocity = (transform.position - lastPosition) / Time.deltaTime;
                lastPosition = transform.position;

                if (beingHeldR && !ControllerInput.instance.GetInput(ControllerInput.InputType.rightGrip))
                {
                    Release(velocity);
                }
                else if (beingHeldL && !ControllerInput.instance.GetInput(ControllerInput.InputType.leftGrip))
                {
                    Release(velocity);
                }
            }
        }

        void TryGrab()
        {
            if (ControllerInput.instance.GetInput(ControllerInput.InputType.rightGrip))
            {
                if (!beingHeldR && Vector3.Distance(Player.Instance.RightHand.transform.position, transform.position) <
                    grabDistance)
                {
                    Grab(Player.Instance.RightHand.transform, false);
                    beingHeldR = true;
                }
            }
            else if (ControllerInput.instance.GetInput(ControllerInput.InputType.leftGrip))
            {
                if (!beingHeldL && Vector3.Distance(Player.Instance.LeftHand.transform.position, transform.position) <
                    grabDistance)
                {
                    Grab(Player.Instance.LeftHand.transform, true);
                    beingHeldL = true;
                }
            }
        }

        void Grab(Transform hand, bool isLeft)
        {
            transform.SetParent(hand, worldPositionStays: true);
            rb.isKinematic = true;
            beingHeldL = isLeft;
            beingHeldR = !isLeft;
            lastPosition = transform.parent.position;

            BallL.SetActive(!isLeft);
            BallR.SetActive(isLeft);
        }

        void Release(Vector3 throwVelocity)
        {
            transform.SetParent(null, worldPositionStays: true);
            rb.isKinematic = false;
            rb.velocity = throwVelocity;
            beingHeldR = false;
            beingHeldL = false;

            BallL.SetActive(true);
            BallR.SetActive(true);
        }
    }
}