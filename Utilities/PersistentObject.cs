// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// Makes a game object persist between scenes.
    /// </summary>
    public class PersistentObject : MonoBehaviour
    {
        #region Fields
        [Tooltip("Scenes the persistant object should not run on.")]
        [SerializeField] private List<string> disableScenes = new List<string>();
        #endregion

        #region Properties
        public List<string> DisableScenes { get => disableScenes; }
        #endregion

        #region Methods
        /// <summary>
        /// Calls InstantiatePersistentObjectsPool.
        /// </summary>
        void Awake()
        {
            InstantiatePersistentObjectsPool();
        }

        /// <summary>
        /// Creates a new Persistent Object Pool if needed, and adds this instance to it.
        /// </summary>
        private void InstantiatePersistentObjectsPool()
        {
            if (PersistentObjectsPool.instance == null)
            {
                PersistentObjectsPool.instance = new PersistentObjectsPool();
            }

            PersistentObjectsPool.instance.InitializeInstance(this);
        }

        /// <summary>
        /// Removes this instance from the pool.
        /// </summary>
        private void OnDestroy()
        {
            PersistentObjectsPool.instance.RemoveObjectFromPool(this);
        }
        #endregion
    }
}