using Banapuchin.Classes;
using Banapuchin.Extensions;
using Banapuchin.Libraries;
using Banapuchin.Mods.Weird;
using UnityEngine;

namespace Banapuchin.Mods.Gun
{
    public class SpectateGun : ModBase
    {
        public override string Text => "Spectate Gun";
        public override List<Type> Incompatibilities => new List<Type> { typeof(Thirdperson) };

        private readonly GunLibrary gun = new GunLibrary
        {
            FollowPlayer = true,
        };

        private static GameObject _cameraObj;

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
            if (gun.IsFiring && gun.SelectedFusionPlayer != null && _cameraObj == null)
            {
                _cameraObj = new GameObject("SpectateCamera");
                Camera camera = _cameraObj.AddComponent<Camera>();
                camera.fieldOfView = 110f;
                camera.nearClipPlane = 0.001f;
                _cameraObj.transform.SetParent(gun.SelectedFusionPlayer.transform.Find("Head"));
                _cameraObj.transform.localPosition = Vector3.zero;
                _cameraObj.transform.localRotation = Quaternion.identity;
                UnityEngine.Object.DontDestroyOnLoad(_cameraObj);
            }

            if (!gun.IsFiring && _cameraObj != null)
            {
                _cameraObj.Obliterate(out _cameraObj);
            }
        }
    }
}