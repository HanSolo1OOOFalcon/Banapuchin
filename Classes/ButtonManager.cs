using System;
using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Classes
{
    public class ButtonManager : BaseButtonManager
    {
        public ModBase modInstance = null;
        private AudioSource audio;

        void Start()
        {
            audio = GetComponent<AudioSource>();
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            var thing = FusionHub.currentQueue;
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