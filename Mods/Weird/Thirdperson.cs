using Banapuchin.Classes;
using Banapuchin.Extensions;
using Locomotion;
using UnityEngine;

namespace Banapuchin.Mods.Weird
{
    public class Thirdperson : ModBase
    {
        public override string Text => "Third Person";

        static GameObject cameraObj;

        public override void OnEnable()
        {
            base.OnEnable();
            cameraObj = new GameObject("SpectateCamera");
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.fieldOfView = 110f;
            camera.nearClipPlane = 0.001f;
            cameraObj.transform.SetParent(Player.Instance.playerCam.gameObject.transform);
            cameraObj.transform.localPosition = new Vector3(0f, 0f, -2f);
            cameraObj.transform.localRotation = Quaternion.identity;
            Object.DontDestroyOnLoad(cameraObj);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            cameraObj.Obliterate(out cameraObj);
        }
    }
}
