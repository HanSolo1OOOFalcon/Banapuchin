using Banapuchin.Main;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Classes
{
    public abstract class ModBase
    {
        public abstract string Text { get; }
        public bool isEnabled = false;

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnEnable()
        {
            ButtonObject.GetComponent<Renderer>().material.color = Color.red;
            Plugin.toInvoke.Add(Update);
            Plugin.toInvokeFixed.Add(FixedUpdate);
            ButtonObject.transform.localPosition = new Vector3(ButtonObject.transform.localPosition.x, -0.0002f, ButtonObject.transform.localPosition.z);
        }
        public virtual void OnDisable()
        {
            ButtonObject.GetComponent<Renderer>().material.color = Color.white * 0.75f;
            Plugin.toInvoke.Remove(Update);
            Plugin.toInvokeFixed.Remove(FixedUpdate);
            ButtonObject.transform.localPosition = new Vector3(ButtonObject.transform.localPosition.y, -0.0004f, ButtonObject.transform.localPosition.z);
        }

        public virtual List<Type> Incompatibilities { get; } = new List<Type>();
        public virtual List<Type> Dependencies { get; } = new List<Type>();

        #region base methods every single mod should have that are called by the button manager component
        public void Toggle()
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                List<Type> mods = new List<Type>();
                foreach (ModBase mod in PublicThingsHerePlease.modInstances)
                {
                    mods.Add(mod.GetType());
                }

                foreach (Type mod in mods)
                {
                    if (Incompatibilities.Contains(mod))
                    {
                        ModBase instance = PublicThingsHerePlease.modInstances.Find(m => m.GetType() == mod);
                        if (instance != null && instance.isEnabled)
                        {
                            instance.OnDisable();
                        }
                    }
                    else if (Dependencies.Contains(mod))
                    {
                        ModBase instance = PublicThingsHerePlease.modInstances.Find(m => m.GetType() == mod);
                        if (instance != null && !instance.isEnabled)
                        {
                            instance.OnEnable();
                        }
                    }
                }

                OnEnable();
            }
            else
            {
                OnDisable();

                List<Type> mods = new List<Type>();
                foreach (ModBase mod in PublicThingsHerePlease.modInstances)
                {
                    mods.Add(mod.GetType());
                }

                foreach (Type mod in mods)
                {
                    ModBase instance = PublicThingsHerePlease.modInstances.Find(m => m.GetType() == mod);
                    if (instance.Dependencies.Contains(this.GetType()) && instance.isEnabled)
                    {
                        instance.OnDisable();
                    }
                }
            }
        }
        #endregion

        // Do not assign these
        public GameObject ButtonObject = null;
        public GameObject TextObject = null;
    }
}