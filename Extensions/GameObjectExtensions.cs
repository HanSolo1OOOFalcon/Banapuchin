using UnityEngine;

namespace Banapuchin.Extensions
{
    public static class GameObjectExtensions
    {
        public static void Obliterate(this GameObject obj, out GameObject result)
        {
            if (obj != null)
            {
                GameObject.Destroy(obj);
            }
            result = null;
        }

        public static T SafelyAddComponent<T>(this GameObject obj) where T : Component // I HAD TO RENAME THIS METHOD BECAUSE A SPECIAL SOMEONE WANTS UTILITY FOR THE NOOBS!!!!!!!!!!!!!!!!!!
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                component = obj.AddComponent<T>();
            }
            return component;
        }

        public static void Reset(this GameObject go)
        {
            go.transform.parent = null;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
    }
}