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
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }

        public virtual List<Type> Incompatibilities { get; } = new List<Type>();
        public virtual List<Type> Dependencies { get; } = new List<Type>();

        // Just some things
        public void Toggle()
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                OnEnable();
                ButtonObject.GetComponent<Renderer>().material.color = Color.red;
                Plugin.toInvoke.Add(Update);
                Plugin.toInvokeFixed.Add(FixedUpdate);
                ButtonObject.transform.localPosition = new Vector3(0.6f, ButtonObject.transform.localPosition.y, ButtonObject.transform.localPosition.z);

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
                            instance.isEnabled = false;
                            instance.OnDisable();
                            instance.ButtonObject.GetComponent<Renderer>().material.color = Color.white * 0.75f;
                            Plugin.toInvoke.Remove(instance.Update);
                            Plugin.toInvokeFixed.Remove(instance.FixedUpdate);
                            instance.ButtonObject.transform.localPosition = new Vector3(1f, instance.ButtonObject.transform.localPosition.y, instance.ButtonObject.transform.localPosition.z);
                        }
                    }
                    else if (Dependencies.Contains(mod))
                    {
                        ModBase instance = PublicThingsHerePlease.modInstances.Find(m => m.GetType() == mod);
                        if (instance != null && !instance.isEnabled)
                        {
                            instance.isEnabled = true;
                            instance.OnEnable();
                            instance.ButtonObject.GetComponent<Renderer>().material.color = Color.red;
                            Plugin.toInvoke.Add(instance.Update);
                            Plugin.toInvokeFixed.Add(instance.FixedUpdate);
                            instance.ButtonObject.transform.localPosition = new Vector3(0.6f, instance.ButtonObject.transform.localPosition.y, instance.ButtonObject.transform.localPosition.z);
                        }
                    }
                }
            }
        }

        // Do not assign these
        public GameObject ButtonObject = null;
        public GameObject TextObject = null;
    }
}
