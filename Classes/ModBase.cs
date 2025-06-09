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

        // Do not assign these
        public GameObject ButtonObject = null;
        public GameObject TextObject = null;
    }
}
