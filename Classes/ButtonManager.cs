using Banapuchin.Main;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class ButtonManager : BaseButtonManager
    {
        public ModBase modInstance = null;

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (isAllowed)
            {
                isAllowed = false;
                
                modInstance.isEnabled = !modInstance.isEnabled;

                if (modInstance.isEnabled)
                {
                    modInstance.OnEnable();
                    modInstance.ButtonObject.GetComponent<Renderer>().material.color = Color.red;
                    Plugin.toInvoke.Add(modInstance.Update);
                    Plugin.toInvokeFixed.Add(modInstance.FixedUpdate);
                    base.transform.localPosition = new Vector3(0.6f, base.transform.localPosition.y, base.transform.localPosition.z);
                }
                else
                {
                    modInstance.OnDisable();
                    modInstance.ButtonObject.GetComponent<Renderer>().material.color = Color.white * 0.75f;
                    Plugin.toInvoke.Remove(modInstance.Update);
                    Plugin.toInvokeFixed.Remove(modInstance.FixedUpdate);
                    base.transform.localPosition = new Vector3(1f, base.transform.localPosition.y, base.transform.localPosition.z);
                }
            }
        }
    }
}