using UnityEngine;
using static Banapuchin.PublicThingsHerePlease;

namespace Banapuchin.Main
{
    public class IrregularButtonMethods
    {
        public static void LastPage()
        {
            currentPage--;
            if (currentPage < 0)
                currentPage = Mathf.CeilToInt(ModInstances.Count/5f) - 1;
            UpdateButtons();
        }

        public static void NextPage()
        {
            currentPage++;
            if (currentPage >= Mathf.CeilToInt(ModInstances.Count/5f))
                currentPage = 0;
            UpdateButtons();
        }
    }
}