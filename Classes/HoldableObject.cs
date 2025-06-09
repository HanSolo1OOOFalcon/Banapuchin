using Caputilla.Utils;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class HoldableObject : BetterMonoBehaviour
    {
        public float grabDistance = 0.2f;
        private Rigidbody rb;
        public bool beingHeldR = false, beingHeldL = false;
        private Transform handHolding;
        private Vector3 lastPosition;
        private Vector3 velocity;

        void Start()
        {
            rb = transform.parent.gameObject.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (!beingHeldR && !beingHeldL)
            {
                TryGrab();
            }
            else
            {
                velocity = (transform.position - lastPosition) / Time.deltaTime;
                lastPosition = transform.position;

                if (beingHeldR && !ControllerInputManager.Instance.rightGrip)
                {
                    Release(velocity);
                }
                else if (beingHeldL && !ControllerInputManager.Instance.leftGrip)
                {
                    Release(velocity);
                }
            }
        }

        void TryGrab()
        {
            if (ControllerInputManager.Instance.rightGrip)
            {
                if (!beingHeldR && Vector3.Distance(Player.Instance.RightHand.transform.position, transform.position) < grabDistance)
                {
                    Grab(Player.Instance.RightHand.transform, false);
                    beingHeldR = true;
                }
            }
            else if (ControllerInputManager.Instance.leftGrip)
            {
                if (!beingHeldL && Vector3.Distance(Player.Instance.LeftHand.transform.position, transform.position) < grabDistance)
                {
                    Grab(Player.Instance.LeftHand.transform, true);
                    beingHeldL = true;
                }
            }
            else
            {
                beingHeldR = false;
                beingHeldL = false;
            }
        }

        void Grab(Transform hand, bool isLeft)
        {
            handHolding = hand;
            transform.parent.SetParent(hand, worldPositionStays: true);
            rb.isKinematic = true;
            beingHeldL = isLeft;
            beingHeldR = !isLeft;
            lastPosition = transform.parent.position;

            PublicThingsHerePlease.lBall.SetActive(!isLeft);
            PublicThingsHerePlease.rBall.SetActive(isLeft);
        }

        void Release(Vector3 throwVelocity)
        {
            transform.parent.SetParent(null, worldPositionStays: true);
            rb.isKinematic = false;
            rb.velocity = throwVelocity;
            beingHeldR = false;
            beingHeldL = false;
            handHolding = null;

            PublicThingsHerePlease.lBall.SetActive(true);
            PublicThingsHerePlease.rBall.SetActive(true);
        }
    }
}
