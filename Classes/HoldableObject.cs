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
        private Vector3 lastPosition;
        private Vector3 velocity;

        void Start()
        {
            rb = transform.gameObject.GetComponent<Rigidbody>();
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
        }

        void Grab(Transform hand, bool isLeft)
        {
            transform.SetParent(hand, worldPositionStays: true);
            rb.isKinematic = true;
            beingHeldL = isLeft;
            beingHeldR = !isLeft;
            lastPosition = transform.parent.position;

            PublicThingsHerePlease.lBall.SetActive(!isLeft);
            PublicThingsHerePlease.rBall.SetActive(isLeft);
        }

        void Release(Vector3 throwVelocity)
        {
            transform.SetParent(null, worldPositionStays: true);
            rb.isKinematic = false;
            rb.velocity = throwVelocity;
            beingHeldR = false;
            beingHeldL = false;

            PublicThingsHerePlease.lBall.SetActive(true);
            PublicThingsHerePlease.rBall.SetActive(true);
        }
    }
}
