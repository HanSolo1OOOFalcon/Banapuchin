using Banapuchin.Extensions;
using UnityEngine;
using Locomotion;
using Banapuchin.Classes;
using Banapuchin.Libraries;

namespace Banapuchin.Mods.Movement
{
    public class Platforms : ModBase
    {
        public override string Text => "Platforms";

        public static GameObject lPlat, rPlat;

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
            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("CapuchinHead");
            GameObject plat = GameObject.Instantiate(foo);
            plat.transform.localScale = Vector3.one * 100f;
            plat.AddComponent<BoxCollider>();
            PublicThingsHerePlease.FixShaders(plat);
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
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.leftGrip))
            {
                lPlat.SetActive(true);
                lPlat.transform.position = Player.Instance.LeftHand.transform.position + Vector3.down * 0.2f;
                lPlat.transform.rotation = Player.Instance.LeftHand.transform.rotation;
            }
            else if (ControllerInput.instance.GetInputUp(ControllerInput.InputType.leftGrip))
            {
                lPlat.SetActive(false);
            }

            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.rightGrip))
            {
                rPlat.SetActive(true);
                rPlat.transform.position = Player.Instance.RightHand.transform.position + Vector3.down * 0.2f;
                rPlat.transform.rotation = Player.Instance.RightHand.transform.rotation;
            }
            else if (ControllerInput.instance.GetInputUp(ControllerInput.InputType.rightGrip))
            {
                rPlat.SetActive(false);
            }
        }
    }
}