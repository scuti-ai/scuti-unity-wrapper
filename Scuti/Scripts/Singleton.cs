using UnityEngine;

namespace Scuti
{
    /// <summary>
    /// Singleton behaviour class, used for components that should only have one instance. 
    /// </summary>
    /// <typeparam name="T">The Singleton Type</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        /// <summary>
        /// Returns the Singleton instance of the classes type.
        /// If no instance is found, then we search for an instance
        /// in the scene.
        /// If more than one instance is found, we throw an error and
        /// no instance is returned.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!IsInitialized && searchForInstance)
                {
                    searchForInstance = false;
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length == 1)
                    {
                        instance = objects[0];
                    }
                    else if (objects.Length > 1)
                    {
                        ScutiLogger.LogError(string.Format("Expected exactly 1 {0} but found {1}.", typeof(T).Name, objects.Length));
                    }
                }
                return instance;
            }
        }

        // Helps insure we only search once
        private static bool searchForInstance = true;


        /// <summary>
        /// Returns whether the instance has been initialized or not.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return instance != null;
            }
        }


        protected virtual void Awake()
        {
            if (IsInitialized && instance != this)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(this);
                }
                else
                {
                    Destroy(this);
                }
                ScutiLogger.LogError(string.Format("Trying to instantiate a second instance of singleton class {0}. Additional Instance was destroyed", GetType().Name));
            }
            else if (!IsInitialized)
            {
                instance = (T)this;
                searchForInstance = false;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
