using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Libraries;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Mods.Gun
{
    public class SpectateGun : ModBase
    {
        public override string Text => "Spectate Gun";

        static GunLibrary gun = new GunLibrary
        {
            followPlayer = true,
        };

        static bool wasFiring;
        static GameObject cameraObj;

        public override void OnEnable()
        {
            gun.OnEnable();
        }

        public override void OnDisable()
        {
            gun.OnDisable();
        }

        public override void Update()
        {
            gun.Forever();
            if (gun.isFiring)
            {
                if (gun.selectedFusionPlayer != null)
                {
                    if (cameraObj == null)
                    {
                        cameraObj = new GameObject("SpectateCamera");
                        Camera camera = cameraObj.AddComponent<Camera>();
                        camera.fieldOfView = 110f;
                        camera.nearClipPlane = 0.001f;
                        cameraObj.transform.position = Player.Instance.playerCam.transform.position;
                        Object.DontDestroyOnLoad(cameraObj);
                    }
                    cameraObj.transform.position = gun.selectedFusionPlayer.transform.Find("Head").position;
                    cameraObj.transform.rotation = gun.selectedFusionPlayer.transform.Find("Head").rotation;
                }
            }

            if (!gun.isFiring && cameraObj != null)
            {
                cameraObj.Obliterate(out cameraObj);
            }

            wasFiring = gun.isFiring;
        }
    }
}
