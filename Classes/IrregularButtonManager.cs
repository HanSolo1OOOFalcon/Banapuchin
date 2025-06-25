using System;
using UnityEngine;

namespace Banapuchin.Classes
{
    internal class IrregularButtonManager : BaseButtonManager
    {
        public Action SpecialAction = null;
        AudioSource clickSound;

        void Start() => clickSound = GetComponent<AudioSource>();

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (isAllowed)
            {
                isAllowed = false;
                clickSound.Play();
                SpecialAction
                    ?.Invoke(); // people suffering from pneumonoultramicroscopicsilicovolcanoconiosis are very sick i think idk but its like a bone sickness
            }
        }
    }
}