using Il2CppLocomotion;
using UnityEngine;
using Banapuchin.Extensions;
using Il2Cpp;

namespace Banapuchin.Libraries
{
    public class GunLibrary
    {
        public RaycastHit Hit;
        public bool IsFiring, FollowPlayer;
        public FusionPlayer SelectedFusionPlayer;
        private GameObject pointer;
        private LineRenderer line;
        private GameObject gunObj;

        public void OnEnable()
        {
            pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointer.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            pointer.GetComponent<Renderer>().material.color = Color.white * 0.75f;
            UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
            pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            pointer.SetActive(false);

            line = pointer.AddComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.material.color = Color.white * 0.75f;
            line.positionCount = 2;

            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("BananaGun");
            gunObj = UnityEngine.Object.Instantiate(original: foo, parent: Player.Instance.RightHand.transform);
            gunObj.transform.localPosition = new Vector3(-0.038f, -0.0325f, 0.012f);
            gunObj.transform.localRotation = Quaternion.Euler(0f, 90f, 45f);
            gunObj.transform.localScale = Vector3.one * 0.175f;
            gunObj.SetActive(false);
            PublicThingsHerePlease.FixShaders(gunObj);
        }

        public void OnDisable()
        {
            if (pointer != null)
                pointer.Obliterate(out pointer);
            
            if (gunObj != null)
                gunObj.Obliterate(out gunObj);
        }

        public void Forever()
        {
            Vector3 direction = -Player.Instance.RightHand.transform.up;
            Vector3 rotatedDirection =
                Quaternion.AngleAxis(-45f, Player.Instance.RightHand.transform.right) * direction;

            /*Vector3 handRight = Quaternion.AngleAxis(45f, Player.Instance.RightHand.transform.right) * -Player.Instance.RightHand.transform.up;*/
            Vector3 offset = new Vector3(-0.81f, 0.5f, 0);
            Vector3
                offsetPosition =
                    gunObj.transform
                        .TransformPoint(
                            offset); /*Player.Instance.RightHand.transform.position + handRight.normalized * 0.0275f;*/

            if (ControllerInput.instance.GetInput(ControllerInput.InputType.RightGrip))
            {
                gunObj.SetActive(true);
                if (Physics.Raycast(offsetPosition, rotatedDirection, out Hit, 1000f))
                {
                    pointer.SetActive(
                        true); // devlog 4. monky said hes gonna teach me active players!!!!!!!!! 2025-06-04 20:14 (YYYY-MM-DD HH:MM) GMT+1

                    if (FollowPlayer) // devlog 1. this feature should be pretty straight forward monky told me how to do it (kind of) so i hope it goes well 2025-06-02 21:53 (YYYY-MM-DD HH:MM) GMT+1
                    {
                        if (ControllerInput.instance.GetInput(ControllerInput.InputType
                                .RightTrigger)) // devlog 2. i was really fucking wrong ive been working on this stupid feature for like 4 hours STRAIGHT and it just wont work please send help all this for a spectate player gun is it really worth it idk 2025-06-03 18:37 (YYYY-MM-DD HH:MM) GMT+1
                        {
                            if (SelectedFusionPlayer ==
                                null) // devlog 3. i really want to work on this but ariel wont hop on!!!!!!!!!!!!!!!!!!!! 2025-06-04 17:26 (YYYY-MM-DD HH:MM) GMT+1
                            {
                                FusionPlayer
                                    viable =
                                        null; // devlog 5. IT WORKS AFTER 2 DAYS!!!!!!!!!!!!!!!!!!!!!!!!!!!! 2025-06-04 21:51 (YYYY-MM-DD HH:MM) GMT+1
                                float closestDistance = float.MaxValue;

                                foreach (var player in FusionHub.Instance.SpawnedPlayers)
                                {
                                    FusionPlayer fusionPlayer = player.Item1;
                                    if (fusionPlayer.IsLocalPlayer) continue;

                                    Transform head = fusionPlayer.transform.Find("Head");
                                    float distance = Vector3.Distance(head.position, Hit.point);
                                    if (distance < closestDistance)
                                    {
                                        closestDistance = distance;
                                        viable = fusionPlayer;
                                    }
                                }

                                SelectedFusionPlayer = viable;
                            }

                            IsFiring = true;
                            pointer.transform.position = SelectedFusionPlayer != null
                                ? SelectedFusionPlayer.transform.Find("Head").position
                                : Hit.point;
                        }
                        else
                        {
                            IsFiring = false;
                            SelectedFusionPlayer = null;
                            pointer.transform.position = Hit.point;
                        }
                    }
                    else
                    {
                        IsFiring = ControllerInput.instance.GetInput(ControllerInput.InputType.RightTrigger);
                        pointer.transform.position = Hit.point;
                    }

                    line.SetPosition(0, offsetPosition);
                    line.SetPosition(1, pointer.transform.position);
                }
            }
            else
            {
                gunObj.SetActive(false);
                pointer.SetActive(false);
                IsFiring = false;
            }
        }
    }
}