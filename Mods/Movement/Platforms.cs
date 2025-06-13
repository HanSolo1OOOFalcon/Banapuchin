using Caputilla.Utils;
using Banapuchin.Extensions;
using UnityEngine;
using Locomotion;
using Banapuchin.Classes;

namespace Banapuchin.Mods.Movement
{
    public class Platforms : ModBase
    {
        public override string Text => "Platforms";

        public static GameObject lPlat, rPlat;
        static bool lastGripLeft = false, lastGripRight = false;

        public override void OnDisable()
        {
            base.OnDisable();
            if (lPlat != null)
            {
                lPlat.Obliterate(out lPlat);
            }
            if (rPlat != null)
            {
                rPlat.Obliterate(out rPlat);
            }
        }

        static GameObject CreatePlat()
        {
            GameObject plat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plat.transform.localScale = new Vector3(0.02f, 0.3f, 0.3f);
            plat.transform.position = new Vector3(-84f, 2f, 95f);

            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("Banana");
            GameObject nanner = GameObject.Instantiate(foo);
            nanner.transform.SetParent(plat.transform);
            nanner.transform.localPosition = Vector3.zero;
            nanner.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            nanner.transform.localScale = Vector3.one;

            plat.SetActive(false);
            return plat;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            lPlat.Obliterate(out lPlat);
            lPlat = CreatePlat();

            rPlat.Obliterate(out rPlat);
            rPlat = CreatePlat();
        }

        public override void Update()
        {
            if (ControllerInputManager.Instance.leftGrip && !lastGripLeft)
            {
                lPlat.SetActive(true);
                lPlat.transform.position = Player.Instance.LeftHand.transform.position + Vector3.down * 0.1f;
                lPlat.transform.rotation = Player.Instance.LeftHand.transform.rotation;
            }
            else if (!ControllerInputManager.Instance.leftGrip && lastGripLeft)
            {
                lPlat.SetActive(false);
            }

            if (ControllerInputManager.Instance.rightGrip && !lastGripRight)
            {
                rPlat.SetActive(true);
                rPlat.transform.position = Player.Instance.RightHand.transform.position + Vector3.down * 0.1f;
                rPlat.transform.rotation = Player.Instance.RightHand.transform.rotation;
            }
            else if (!ControllerInputManager.Instance.rightGrip && lastGripRight)
            {
                rPlat.SetActive(false);
            }

            lastGripRight = ControllerInputManager.Instance.rightGrip;
            lastGripLeft = ControllerInputManager.Instance.leftGrip;
        }
    }
}
