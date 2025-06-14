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

            GameObject menuThing = bundle.LoadAsset<GameObject>("BanapuchinMenu");
            menu = Instantiate(menuThing);
            menu.transform.localScale = Vector3.one * 25f;
            menu.AddComponent<Rigidbody>().isKinematic = false;
            menu.AddComponent<HoldableObject>();
            FixShaders(menu);
            menu.name = "BanapuchinModMenu";

            AudioClip clickSound = bundle.LoadAsset<AudioClip>("ButtonPressWood");

            // this shit way too formal yall know i dont follow suite
            GameObject pageL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageL.transform.SetParent(menu.transform);
            pageL.transform.localScale = new Vector3(0.004f, 0.002f, 0.004f);
            pageL.transform.localRotation = Quaternion.identity;
            pageL.transform.localPosition = new Vector3(0.004f, 0f, 0.012f);
            pageL.AddComponent<AudioSource>().clip = clickSound;
            pageL.GetComponent<AudioSource>().playOnAwake = false;
            pageL.GetComponent<BoxCollider>().isTrigger = true;
            pageL.AddComponent<IrregularButtonManager>().SpecialAction = () => IrregularButtonMethods.LastPage();

            GameObject pageR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pageR.transform.SetParent(menu.transform);
            pageR.transform.localScale = new Vector3(0.004f, 0.002f, 0.004f);
            pageR.transform.localRotation = Quaternion.identity;
            pageR.transform.localPosition = new Vector3(-0.004f, 0f, 0.012f);
            pageR.AddComponent<AudioSource>().clip = clickSound;
            pageR.GetComponent<AudioSource>().playOnAwake = false;
            pageR.GetComponent<BoxCollider>().isTrigger = true;
            pageR.AddComponent<IrregularButtonManager>().SpecialAction = () => IrregularButtonMethods.NextPage();

            // SUPER CUTE CAT IMAGES YAYAYAYAYAYAYAYAYAYAYAY
            LoadImageInto3DWorldSpace("Banapuchin.Assets.Cats.MonkyCar.jpg", menu.transform, new Vector3(0f, 0.0019f, 0f), Quaternion.Euler(90f, 0f, 0f), Vector3.one * 0.01f);

            // butts (haha i said funneh word)
            var ModBaseType = typeof(ModBase);
            var Mods = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(ModBaseType)).ToArray();
            Plugin.instance.WriteLine($"Loaded {Mods.Length} mods, if this seems incorrect then you fucked up. Maybe try inheriting from the class?", LogLevel.Warning);
            GameObject buttonPrefab = bundle.LoadAsset<GameObject>("BanapuchinButton");

            for (int buttonIndex = 0; buttonIndex < Mods.Length; buttonIndex++)
            {
                float offset = 0.003f * (buttonIndex % 5);

                Type modType = Mods[buttonIndex];
                ModBase instance = (ModBase)Activator.CreateInstance(modType);

                modInstances.Add(instance);

                CreateButton(offset, instance, buttonPrefab, clickSound);
            }
            UpdateButtons();
        }

        static void CreateButton(float offset, ModBase instance, GameObject toInstantiate, AudioClip clickSound)
        {
            GameObject button = Instantiate(toInstantiate);
            button.transform.SetParent(menu.transform);
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
            buttonManager.modInstance = instance;

            instance.ButtonObject = button;
            instance.TextObject = CreateTextLabel(instance.Text, button.transform, new Vector3(0f, 0f, 0.007f), Quaternion.Euler(0f, 180f, 270f), 0.125f);
        }

        private static GameObject CreateTextLabel(string text, Transform parent, Vector3 pos, Quaternion rot, float size)
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
            menu.Obliterate(out menu);
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
            if (!allowed) 
                return;

            if (ControllerInputManager.Instance.rightSecondary && !wasPressed)
            {
                menu.GetComponent<Rigidbody>().isKinematic = true;
                menu.transform.SetParent(Player.Instance.playerCam.gameObject.transform);
                menu.transform.localPosition = new Vector3(0f, -0.04f, 0.6f);
                menu.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);
                menu.transform.localScale = Vector3.one * 25f;
                lBall.SetActive(true);
                rBall.SetActive(true);
                menu.SetActive(true);
            }
            wasPressed = ControllerInputManager.Instance.rightSecondary;

            if (!menu.GetComponent<Rigidbody>().isKinematic)
            {
                lBall.SetActive(false);
                rBall.SetActive(false);
            }

            foreach (Action action in toInvoke)
            {
                action.Invoke();
            }
        }

        void FixedUpdate()
        {
            if (!allowed) 
                return;
            foreach (Action action in toInvokeFixed)
            {
                action.Invoke();
            }
        }
    }
}