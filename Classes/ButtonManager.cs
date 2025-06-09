using Banapuchin.Main;
using System;
using System.Collections.Generic;
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
                modInstance.Toggle();
            }
        }
    }
}