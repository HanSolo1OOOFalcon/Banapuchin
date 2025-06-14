using Banapuchin.Main;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class ButtonManager : BaseButtonManager
    {
        public ModBase modInstance = null;
        AudioSource audio;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (isAllowed)
            {
                isAllowed = false;
                modInstance.Toggle();
                audio.Play();
            }
        }
    }
}