using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Main
{
    internal class IrregularButtonMethods
    {
        public static void LastPage()
        {
            currentPage--;
            if (currentPage < 0)
            {
                currentPage = Mathf.CeilToInt((float)modInstances.Count/5f) - 1;
            }
            UpdateButtons();
        }

        public static void NextPage()
        {
            currentPage++;
            if (currentPage >= Mathf.CeilToInt((float)modInstances.Count/5f))
            {
                currentPage = 0;
            }
            UpdateButtons();
        }
    }
}
