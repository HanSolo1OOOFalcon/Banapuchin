using Banapuchin.Main;
using Il2Cpp;
using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Classes
{
    public abstract class ModBase
    {
        public abstract string Text { get; }
        public bool IsEnabled;

        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void OnEnable()
        {
            ButtonObject.GetComponent<Renderer>().material.color = Color.red;
            MainMod.ToInvoke.Add(Update);
            MainMod.ToInvokeFixed.Add(FixedUpdate);
        }
        public virtual void OnDisable()
        {
            ButtonObject.GetComponent<Renderer>().material.color = Color.white * 0.75f;
            MainMod.ToInvoke.Remove(Update);
            MainMod.ToInvokeFixed.Remove(FixedUpdate);
        }

        public virtual List<Type> Incompatibilities { get; } = new List<Type>();
        public virtual List<Type> Dependencies { get; } = new List<Type>();

        #region base methods every single mod should have that are called by the button manager component
        public void Toggle()
        {
            IsEnabled = !IsEnabled;

            if (IsEnabled)
            {
                List<Type> mods = new List<Type>();
                foreach (ModBase mod in ModInstances)
                {
                    mods.Add(mod.GetType());
                }

                foreach (Type mod in mods)
                {
                    if (Incompatibilities.Contains(mod))
                    {
                        ModBase instance = ModInstances.Find(m => m.GetType() == mod);
                        if (instance != null && instance.IsEnabled)
                        {
                            instance.OnDisable();
                            instance.IsEnabled = false;
                        }
                    }
                    else if (Dependencies.Contains(mod))
                    {
                        ModBase instance = ModInstances.Find(m => m.GetType() == mod);
                        if (instance != null && !instance.IsEnabled)
                        {
                            instance.OnEnable();
                            instance.IsEnabled = true;
                        }
                    }
                }

                OnEnable();
            }
            else
            {
                OnDisable();

                List<Type> mods = new List<Type>();
                foreach (ModBase mod in ModInstances)
                    mods.Add(mod.GetType());

                foreach (Type mod in mods)
                {
                    ModBase instance = ModInstances.Find(m => m.GetType() == mod);
                    if (instance.Dependencies.Contains(GetType()) && instance.IsEnabled)
                        instance.OnDisable();
                }
            }
        }
        #endregion

        // Do not assign these
        public GameObject ButtonObject = null;
    }
}