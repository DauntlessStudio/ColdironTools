// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// The behind the scenes control to allow persistent objects to work.
    /// </summary>
    public class PersistentObjectsPool
    {
        #region Fields
        public static PersistentObjectsPool instance;
        private List<PersistentObject> persistentObjects = new List<PersistentObject>();
        #endregion

        #region Methods
        /// <summary>
        /// Registers a listener to the scene changing.
        /// </summary>
        public PersistentObjectsPool()
        {
            SceneManager.activeSceneChanged += OnNewScene;
        }

        /// <summary>
        /// Unregisters a listener to the scene changing. Prevents null reference exceptions.
        /// </summary>
        ~PersistentObjectsPool()
        {
            SceneManager.activeSceneChanged -= OnNewScene;
        }

        /// <summary>
        /// Enables/Disables the persistent objects if the new scene is one of it's disabled scenes.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnNewScene(Scene arg1, Scene arg2)
        {
            foreach (PersistentObject persistentObject in persistentObjects)
            {
                persistentObject.gameObject.SetActive(persistentObject.DisableScenes.Contains(arg2.name) ? false : true);
            }
        }

        /// <summary>
        /// Initializes a new persistent object.
        /// </summary>
        /// <param name="persistentObject">The object being initialized.</param>
        public void InitializeInstance(PersistentObject persistentObject)
        {
            if (FoundDuplicate(persistentObject))
            {
                Object.Destroy(persistentObject.gameObject);
                return;
            }

            UnparentObject(persistentObject);

            AddObjectToPool(persistentObject);
        }

        /// <summary>
        /// Removes duplicates of persistent objects.
        /// </summary>
        /// <param name="persistentObject">The object to check for duplicates.</param>
        /// <returns>Bool. True if duplicate was found.</returns>
        private bool FoundDuplicate(PersistentObject persistentObject)
        {
            foreach (PersistentObject item in persistentObjects)
            {
                if (item.gameObject.name == persistentObject.gameObject.name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unparents peristent objects. (Objects must be at the hierarchy root to be persistent.)
        /// </summary>
        /// <param name="persistentObject">The object to unparent.</param>
        private void UnparentObject(PersistentObject persistentObject)
        {
            if (persistentObject.transform.parent) persistentObject.transform.SetParent(null);
        }

        /// <summary>
        /// Adds an object to the pool.
        /// </summary>
        /// <param name="persistentObject">The object to add.</param>
        private void AddObjectToPool(PersistentObject persistentObject)
        {
            persistentObjects.Add(persistentObject);
            Object.DontDestroyOnLoad(persistentObject.gameObject);
        }

        /// <summary>
        /// Removes an object from the pool.
        /// </summary>
        /// <param name="persistentObject">The object to remove.</param>
        public void RemoveObjectFromPool(PersistentObject persistentObject)
        {
            persistentObjects.Remove(persistentObject);
        }
        #endregion
    }
}