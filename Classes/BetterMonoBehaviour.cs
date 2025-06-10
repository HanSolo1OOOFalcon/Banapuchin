using Banapuchin.Main;
using UnityEngine;

namespace Banapuchin.Classes
{
    public class BetterMonoBehaviour : MonoBehaviour
    {
        // guys trust its better
        // lemme cook and make it better
        public virtual void IUseArchBTW()
        {
            Plugin.instance.WriteLine("i use arch btw", BepInEx.Logging.LogLevel.Warning);
        }
    }
}