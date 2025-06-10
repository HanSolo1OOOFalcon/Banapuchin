using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Libraries;
using Banapuchin.Mods.Weird;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Mods.Gun
{
    public class SpectateGun : ModBase
    {
        public override string Text => "Spectate Gun";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Thirdperson) };

        static GunLibrary gun = new GunLibrary
        {
            followPlayer = true,
        };

        static bool wasFiring;
        static GameObject cameraObj;

        public override void OnEnable()
        {
            base.OnEnable();
            gun.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
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
                        cameraObj.transform.SetParent(gun.selectedFusionPlayer.transform.Find("Head"));
                        cameraObj.transform.localPosition = Vector3.zero;
                        cameraObj.transform.localRotation = Quaternion.identity;
                        UnityEngine.Object.DontDestroyOnLoad(cameraObj);
                    }
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
