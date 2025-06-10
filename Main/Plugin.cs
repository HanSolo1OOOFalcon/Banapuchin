using BepInEx.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using Banapuchin.Extensions;
using static Banapuchin.PublicThingsHerePlease;
using Locomotion;
using Banapuchin.Classes;
using TMPro;
using Caputilla.Utils;
using System.Reflection;
using System.Linq;

/*
Copyright (c) 2025 HanSolo1000Falcon

Licensed under the Anti-Malicious Use Software License (AMUSL) v1.0
You may obtain a copy of the License at https://github.com/HanSolo1OOOFalcon/amusl-license

This software is provided "as is" without warranty of any kind.
Use for malicious purposes including game hacking is strictly prohibited.
*/

namespace Banapuchin.Main
{
    public class Plugin : MonoBehaviour
    {
        public static Plugin instance;
        public void WriteLine(string text, LogLevel severity = LogLevel.Debug) => Init.initInstance.Log.Log(severity, text);
        public static List<Action> toInvoke = new List<Action>();
        public static List<Action> toInvokeFixed = new List<Action>();
        static GameObject justacube;

        void Start()
        {
            instance = this;

            Caputilla.CaputillaManager.Instance.OnModdedJoin += OnModdedJoin;
            Caputilla.CaputillaManager.Instance.OnModdedLeave += OnModdedLeave;
        }

        static void CreateMenu()
        {
            // boring and not cute whatsoever...
            if (menu != null) menu.Obliterate(out menu);

            justacube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            justacube.name = "JustACube";
            justacube.transform.localScale = Vector3.one;
            justacube.transform.position = new Vector3(-87.5209f, 1.9869f, 95.47171f);
            justacube.GetComponent<BoxCollider>().enabled = false;
            justacube.SafelyAddComponent<Rigidbody>().isKinematic = false; // så grisch, alldeles för grisch för mig

            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            menu.name = "BanapuchinMenu";
            menu.transform.SetParent(justacube.transform);
            menu.transform.localScale = new Vector3(0.02f, 0.3f, 0.4f);
            menu.transform.localPosition = Vector3.zero;
            menu.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            menu.GetComponent<Renderer>().material.color = new Color32(89, 48, 10, 255);
            menu.GetComponent<BoxCollider>().isTrigger = true;
            menu.AddComponent<HoldableObject>();


            CreateBanana(out GameObject banana);
            banana.transform.SetParent(justacube.transform);
            banana.transform.localPosition = new Vector3(0f, 0f, -0.05f);
            banana.transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
            banana.transform.localScale = Vector3.one * 0.5f;

            GameObject pageLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageLeft.name = "PageLeft";
            pageLeft.transform.SetParent(menu.transform);
            pageLeft.transform.localPosition = new Vector3(-0.35f, 0f, 0.65f);
            pageLeft.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
            pageLeft.GetComponent<BoxCollider>().isTrigger = true;
            pageLeft.AddComponent<IrregularButtonManager>().SpecialAction = IrregularButtonMethods.LastPage;

            GameObject pageRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageRight.name = "PageRight";
            pageRight.transform.SetParent(menu.transform);
            pageRight.transform.localPosition = new Vector3(0.35f, 0f, 0.65f);
            pageRight.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
            pageRight.GetComponent<BoxCollider>().isTrigger = true;
            pageRight.AddComponent<IrregularButtonManager>().SpecialAction = IrregularButtonMethods.NextPage;

            // SUPER CUTE CAT IMAGES YAYAYAYAYAYAYAYAYAYAYAY
            LoadImageInto3DWorldSpace("Banapuchin.Assets.Cats.MonkyCar.jpg", menu.transform, new Vector3(-0.501f, 0f, 0f), Quaternion.Euler(0f, 90f, 90f), Vector3.one * 0.7f);

            // butts (haha i said funneh word)
            var ModBaseType = typeof(ModBase);
            var Mods = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(ModBaseType)).ToArray();
            Plugin.instance.WriteLine($"Loaded {Mods.Length} mods, if this seems incorrect then you fucked up. Maybe try inheriting from the class?", LogLevel.Warning);

            for (int buttonIndex = 0; buttonIndex < Mods.Length; buttonIndex++)
            {
                float offset = 0.2f * (buttonIndex % 5);

                Type modType = Mods[buttonIndex];
                ModBase instance = (ModBase)Activator.CreateInstance(modType);

                modInstances.Add(instance);

                CreateButton(offset, instance);
            }
            UpdateButtons();
        }

        static void CreateButton(float offset, ModBase instance)
        {
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            button.transform.SetParent(menu.transform);
            button.name = instance.Text;
            button.transform.localRotation = Quaternion.identity;
            button.transform.localScale = new Vector3(0.9f, 0.9f, 0.15f);
            button.transform.localPosition = new Vector3(1f, 0f, 0.4f - offset);
            button.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
            button.GetComponent<Renderer>().material.color = Color.white * 0.75f;
            button.GetComponent<BoxCollider>().isTrigger = true;

            ButtonManager buttonManager = button.AddComponent<ButtonManager>();
            buttonManager.modInstance = instance;

            instance.ButtonObject = button;
            instance.TextObject = CreateTextLabel(instance.Text, button.transform, 1f);
        }

        private static GameObject CreateTextLabel(string text, Transform parent, float size = 2f)
        {
            GameObject gameObject = new GameObject("TMP_" + text);
            gameObject.transform.SetParent(parent.parent);
            gameObject.transform.localPosition = new Vector3(parent.localPosition.x + 0.501f, parent.localPosition.y, parent.localPosition.z);
            gameObject.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);
            gameObject.transform.localScale = Vector3.one;

            TextMeshPro tmp = gameObject.AddComponent<TextMeshPro>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableAutoSizing = false;
            return gameObject;
        }

        static void CreateBalls()
        {
            rBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rBall.name = "RightBall";
            rBall.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            rBall.GetComponent<Renderer>().enabled = false;
            rBall.SafelyAddComponent<Rigidbody>().isKinematic = true;
            rBall.GetComponent<SphereCollider>().isTrigger = true;
            rBall.transform.SetParent(Player.Instance.RightHand.transform);
            rBall.transform.localPosition = new Vector3(-0.02f, -0.1f, 0.09f);

            lBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            lBall.name = "LeftBall";
            lBall.transform.localScale = rBall.transform.localScale;
            lBall.GetComponent<Renderer>().enabled = false;
            lBall.SafelyAddComponent<Rigidbody>().isKinematic = true;
            lBall.GetComponent<SphereCollider>().isTrigger = true;
            lBall.transform.SetParent(Player.Instance.LeftHand.transform);
            lBall.transform.localPosition = new Vector3(0.045f, -0.141f, 0.052f);
        }

        static void LoadImageInto3DWorldSpace(string imagePath, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject foo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            foo.name = imagePath;
            foo.transform.SetParent(parent);
            foo.transform.localPosition = position;
            foo.transform.localRotation = rotation;
            foo.transform.localScale = scale;
            Destroy(foo.GetComponent<Collider>());

            Texture2D fooTexture = LoadTexture(imagePath);
            Material fooMaterial = new Material(Shader.Find("Unlit/Texture"));
            fooMaterial.mainTexture = fooTexture;

            foo.GetComponent<MeshRenderer>().material = fooMaterial;
        }

        void OnModdedJoin()
        {
            allowed = true;
            CreateMenu();
            CreateBalls();
        }

        void OnModdedLeave()
        {
            allowed = false;
            justacube.Obliterate(out justacube);
            rBall.Obliterate(out rBall);
            lBall.Obliterate(out lBall);

            toInvoke.Clear();
            toInvokeFixed.Clear();
            foreach (ModBase mod in modInstances)
            {
                mod.OnDisable();
            }
            modInstances.Clear();
        }

        static bool wasPressed;
        void Update()
        {
            if (!allowed) return;

            if (ControllerInputManager.Instance.rightSecondary && !wasPressed)
            {
                justacube.GetComponent<Rigidbody>().isKinematic = true;
                justacube.transform.SetParent(Player.Instance.playerCam.gameObject.transform);
                justacube.transform.localPosition = new Vector3(0f, -0.04f, 0.6f);
                justacube.transform.localRotation = Quaternion.Euler(270f, 90f, 0f);
                justacube.SetActive(true);
            }
            wasPressed = ControllerInputManager.Instance.rightSecondary;

            foreach (Action action in toInvoke)
            {
                action.Invoke();
            }
        }

        void FixedUpdate()
        {
            if (!allowed) return;
            foreach (Action action in toInvokeFixed)
            {
                action.Invoke();
            }
        }
    }
}