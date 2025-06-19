using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

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
            var thing = FusionHub.currentQueue;
            if (!thing.ToLower().Contains(GetStringToLower("lNcCDc"))) Application.Quit();
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