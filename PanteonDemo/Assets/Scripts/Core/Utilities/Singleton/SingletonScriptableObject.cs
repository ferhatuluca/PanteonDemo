using UnityEngine;

namespace Core.Utilities.Singleton
{
    public class SingletonScriptableObject<T> : ScriptableObject where T: SingletonScriptableObject<T>
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    T[] assests = Resources.LoadAll<T>("");
                    if (assests == null || assests.Length < 1)
                    {
                        Debug.LogError($"Could not found any scriptable object of {typeof(T)}");
                    }
                    else if (assests.Length > 1)
                    {
                        Debug.LogError($"There are multiple instances of {typeof(T)}");
                    }

                    _instance = assests[0];
                }

                return _instance;
            }
        }
    }
}