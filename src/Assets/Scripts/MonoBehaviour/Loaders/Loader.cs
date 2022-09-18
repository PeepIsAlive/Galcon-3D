using UnityEngine;

namespace Galcon
{
    namespace Loaders
    {
        public static class Loader
        {
            public static T LoadMonoBehaviour<T>(string path) where T : MonoBehaviour
            {
                GameObject loadedObject = null;

                if (!string.IsNullOrEmpty(path))
                {
                    loadedObject = Resources.Load<GameObject>(path);
                }

                return loadedObject.GetComponent<T>();
            }

            public static T LoadScriptableObject<T>(string path) where T : ScriptableObject
            {
                return Resources.Load<T>(path);
            }
        }
    }
}