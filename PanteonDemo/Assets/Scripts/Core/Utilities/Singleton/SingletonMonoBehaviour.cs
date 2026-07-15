using UnityEngine;

namespace Core.Utilities.Singleton
{
    /// <summary>
    /// Singleton class
    /// </summary>
    /// <typeparam name="T">Type of the SingletonMonoBehaviour</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary>
        /// The static reference to the instance
        /// </summary>
        public static T Instance { get; protected set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                DestroyImmediate(gameObject);
            } 
            else 
            { 
                Instance = (T)this;
                InternalAwake();
            }
        }

        protected virtual void InternalAwake()
        {
        }

        /// <summary>
        /// OnDestroy method to clear singleton association
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}