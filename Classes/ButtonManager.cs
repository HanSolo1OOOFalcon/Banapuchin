using UnityEngine;

namespace Banapuchin.Classes
{
    public class ButtonManager : BaseButtonManager
    {
        public ModBase ModInstance = null;
        private AudioSource audio;

        void Start() => audio = GetComponent<AudioSource>();

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (isAllowed)
            {
                isAllowed = false;
                ModInstance.Toggle();
                audio.Play();
            }
        }
    }
}