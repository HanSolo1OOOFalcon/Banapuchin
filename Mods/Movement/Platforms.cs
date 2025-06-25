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

        private static GameObject _lPlat, _rPlat;

        public override void OnDisable()
        {
            base.OnDisable();
            if (_lPlat != null)
                _lPlat.Obliterate(out _lPlat);

            if (_rPlat != null)
                _rPlat.Obliterate(out _rPlat);
        }

        private static GameObject CreatePlat()
        {
            GameObject foo = PublicThingsHerePlease.bundle.LoadAsset<GameObject>("CapuchinHead");
            GameObject plat = Object.Instantiate(foo);
            plat.transform.localScale = Vector3.one * 100f;
            PublicThingsHerePlease.FixShaders(plat);

            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.SetParent(plat.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(0.0005f, 0.0025f, 0.0025f);
            go.transform.localRotation = Quaternion.identity;

            plat.SetActive(false);
            return plat;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _lPlat.Obliterate(out _lPlat);
            _lPlat = CreatePlat();

            _rPlat.Obliterate(out _rPlat);
            _rPlat = CreatePlat();
        }

        public override void Update()
        {
            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.leftGrip))
            {
                _lPlat.SetActive(true);
                _lPlat.transform.position = Player.Instance.LeftHand.transform.position + Vector3.down * 0.2f;
                _lPlat.transform.rotation = Player.Instance.LeftHand.transform.rotation;
            }
            else if (ControllerInput.instance.GetInputUp(ControllerInput.InputType.leftGrip))
            {
                _lPlat.SetActive(false);
            }

            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.rightGrip))
            {
                _rPlat.SetActive(true);
                _rPlat.transform.position = Player.Instance.RightHand.transform.position + Vector3.down * 0.2f;
                _rPlat.transform.rotation = Player.Instance.RightHand.transform.rotation;
            }
            else if (ControllerInput.instance.GetInputUp(ControllerInput.InputType.rightGrip))
            {
                _rPlat.SetActive(false);
            }
        }
    }
}