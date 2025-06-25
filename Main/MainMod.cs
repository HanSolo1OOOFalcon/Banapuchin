using System.Reflection;
using Banapuchin;
using Banapuchin.Classes;
using Banapuchin.Main;
using MelonLoader;
using static Banapuchin.PublicThingsHerePlease;
using UnityEngine;
using Banapuchin.Extensions;
using Banapuchin.Libraries;
using Banapuchin.Mods.Movement;
using Il2CppInterop.Runtime.Injection;
using Il2CppLocomotion;
using Il2CppTMPro;

[assembly: MelonInfo(typeof(MainMod), ModInfo.NAME, ModInfo.VERSION, ModInfo.AUTHOR)]

namespace Banapuchin.Main
{
    public class MainMod : MelonMod
    {
        public static List<Action> ToInvoke = new List<Action>();
        public static List<Action> ToInvokeFixed = new List<Action>();
        private static bool _hasInit;

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp(typeof(BetterMonoBehaviour));
            var monoTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                t.IsClass && !t.IsAbstract && typeof(BetterMonoBehaviour).IsAssignableFrom(t) &&
                t != typeof(BetterMonoBehaviour)).ToArray();
            foreach (var type in monoTypes)
                ClassInjector.RegisterTypeInIl2Cpp(type);

            Caputilla.Caputilla.Instance.OnModdedJoin += OnModdedJoin;
            Caputilla.Caputilla.Instance.OnModdedLeave += OnModdedLeave;
        }

        private static void Init()
        {
            GameObject addComponentsToMe = new GameObject("BanapuchinComponents");
            addComponentsToMe.AddComponent<HapticLibrary>();
            addComponentsToMe.AddComponent<ControllerInput>();
            addComponentsToMe.AddComponent<CoroutineManager>();

            bundle = LoadAssetBundle("Banapuchin.Assets.modmenu");

            Bouncy.normal = Player.Instance.climbDrag;
            UltraBouncy.normal = Player.Instance.climbDrag;
        }

        private static void CreateMenu()
        {
            // boring and not cute whatsoever...
            if (Menu != null) Menu.Obliterate(out Menu);

            GameObject menuThing = bundle.LoadAsset<GameObject>("BanapuchinMenu");
            Menu = UnityEngine.Object.Instantiate(menuThing);
            Menu.transform.localScale = Vector3.one * 25f;
            Menu.AddComponent<Rigidbody>().isKinematic = false;
            Menu.AddComponent<HoldableObject>();
            FixShaders(Menu);
            Menu.name = "BanapuchinModMenu";

            AudioClip clickSound = bundle.LoadAsset<AudioClip>("ButtonPressWood");

            // this shit way too formal yall know i dont follow suite
            GameObject pageL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageL.transform.SetParent(Menu.transform);
            pageL.transform.localScale = new Vector3(0.004f, 0.002f, 0.004f);
            pageL.transform.localRotation = Quaternion.identity;
            pageL.transform.localPosition = new Vector3(0.004f, 0f, 0.012f);
            pageL.AddComponent<AudioSource>().clip = clickSound;
            pageL.GetComponent<AudioSource>().playOnAwake = false;
            pageL.GetComponent<BoxCollider>().isTrigger = true;
            pageL.AddComponent<IrregularButtonManager>().SpecialAction = () => IrregularButtonMethods.LastPage();

            GameObject pageR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageR.transform.SetParent(Menu.transform);
            pageR.transform.localScale = new Vector3(0.004f, 0.002f, 0.004f);
            pageR.transform.localRotation = Quaternion.identity;
            pageR.transform.localPosition = new Vector3(-0.004f, 0f, 0.012f);
            pageR.AddComponent<AudioSource>().clip = clickSound;
            pageR.GetComponent<AudioSource>().playOnAwake = false;
            pageR.GetComponent<BoxCollider>().isTrigger = true;
            pageR.AddComponent<IrregularButtonManager>().SpecialAction = () => IrregularButtonMethods.NextPage();

            // SUPER CUTE CAT IMAGES YAYAYAYAYAYAYAYAYAYAYAY
            LoadImageInto3DWorldSpace("Banapuchin.Assets.Cats.MonkyCar.jpg", Menu.transform,
                new Vector3(0f, 0.0019f, 0f), Quaternion.Euler(90f, 0f, 0f), Vector3.one * 0.01f);

            // butts (haha i said funneh word)
            var mods = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ModBase))).ToArray();
            Debug.Log(
                $"Loaded {mods.Length} mods, if this seems incorrect then you fucked up. Maybe try inheriting from the class?");
            GameObject buttonPrefab = bundle.LoadAsset<GameObject>("BanapuchinButton");

            for (int buttonIndex = 0; buttonIndex < mods.Length; buttonIndex++)
            {
                float offset = 0.003f * (buttonIndex % 5);

                Type modType = mods[buttonIndex];
                ModBase instance = (ModBase)Activator.CreateInstance(modType);

                ModInstances.Add(instance);

                CreateButton(offset, instance, buttonPrefab, clickSound);
            }

            UpdateButtons();
        }

        private static void CreateButton(float offset, ModBase instance, GameObject toInstantiate, AudioClip clickSound)
        {
            GameObject button = UnityEngine.Object.Instantiate(original: toInstantiate, parent: Menu.transform);
            button.name = instance.Text;
            button.transform.localRotation = Quaternion.Euler(90f, 90f, 0f);
            button.transform.localScale = Vector3.one * 0.12f;
            button.transform.localPosition = new Vector3(0f, -0.0004f, 0.0065f - offset);
            button.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            button.GetComponent<Renderer>().material.color = Color.white * 0.75f;
            button.AddComponent<BoxCollider>().isTrigger = true;

            button.AddComponent<AudioSource>().clip = clickSound;
            button.GetComponent<AudioSource>().playOnAwake = false;

            ButtonManager buttonManager = button.AddComponent<ButtonManager>();
            buttonManager.ModInstance = instance;

            instance.ButtonObject = button;
            CreateTextLabel(instance.Text, button.transform, new Vector3(0f, 0f, 0.007f),
                Quaternion.Euler(0f, 180f, 270f), 0.125f);
        }

        private static GameObject CreateTextLabel(string text, Transform parent, Vector3 pos, Quaternion rot,
            float size)
        {
            GameObject gameObject = new GameObject("TMP_" + text);
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = pos;
            gameObject.transform.localRotation = rot;
            gameObject.transform.localScale = Vector3.one;

            TextMeshPro tmp = gameObject.AddComponent<TextMeshPro>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableAutoSizing = false;

            return gameObject;
        }

        private static void CreateBalls()
        {
            BallR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            BallR.name = "RightBall";
            BallR.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            BallR.GetComponent<Renderer>().enabled = false;
            BallR.SafelyAddComponent<Rigidbody>().isKinematic = true;
            BallR.GetComponent<SphereCollider>().isTrigger = true;
            BallR.transform.SetParent(Player.Instance.RightHand.transform);
            BallR.transform.localPosition = new Vector3(-0.02f, -0.1f, 0.09f);

            BallL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            BallL.name = "LeftBall";
            BallL.transform.localScale = BallR.transform.localScale;
            BallL.GetComponent<Renderer>().enabled = false;
            BallL.SafelyAddComponent<Rigidbody>().isKinematic = true;
            BallL.GetComponent<SphereCollider>().isTrigger = true;
            BallL.transform.SetParent(Player.Instance.LeftHand.transform);
            BallL.transform.localPosition = new Vector3(0.045f, -0.141f, 0.052f);
        }

        private static void LoadImageInto3DWorldSpace(string imagePath, Transform parent, Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            GameObject foo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            foo.name = imagePath;
            foo.transform.SetParent(parent);
            foo.transform.localPosition = position;
            foo.transform.localRotation = rotation;
            foo.transform.localScale = scale;
            UnityEngine.Object.Destroy(foo.GetComponent<Collider>());

            Texture2D fooTexture = LoadTexture(imagePath);
            Material fooMaterial = new Material(Shader.Find("Unlit/Texture"));
            fooMaterial.mainTexture = fooTexture;

            foo.GetComponent<MeshRenderer>().material = fooMaterial;
        }

        private static void OnModdedJoin()
        {
            Allowed = true;
            CreateMenu();
            CreateBalls();
        }

        private static void OnModdedLeave()
        {
            Allowed = false;

            foreach (ModBase mod in ModInstances)
            {
                if (mod.IsEnabled)
                    mod.Toggle();
            }

            Menu.Obliterate(out Menu);
            BallR.Obliterate(out BallR);
            BallL.Obliterate(out BallL);
            ModInstances.Clear();
        }

        public override void OnUpdate()
        {
            if (!Allowed)
                return;

            if (ControllerInput.instance.GetInputDown(ControllerInput.InputType.rightSecondaryButton))
            {
                Menu.GetComponent<Rigidbody>().isKinematic = true;
                Menu.transform.SetParent(Player.Instance.playerCam.gameObject.transform);
                Menu.transform.localPosition = new Vector3(0f, -0.04f, 0.6f);
                Menu.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);
                Menu.transform.localScale = Vector3.one * 25f;
                BallL.SetActive(true);
                BallR.SetActive(true);
                Menu.SetActive(true);
            }

            if (!Menu.GetComponent<Rigidbody>().isKinematic)
            {
                BallL.SetActive(false);
                BallR.SetActive(false);
            }

            foreach (Action action in ToInvoke)
                action.Invoke();
        }

        public override void OnFixedUpdate()
        {
            if (!_hasInit && Player.Instance != null)
            {
                _hasInit = true;
                Init();
            }

            if (!Allowed)
                return;

            foreach (Action action in ToInvokeFixed)
                action.Invoke();
        }
    }
}