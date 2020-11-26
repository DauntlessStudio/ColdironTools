using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Utilities
{
    public class PersistentObject : MonoBehaviour
    {
        #region Fields
        [SerializeField, Tooltip("Scenes the persistant object should not run on")] private List<string> disableScenes = new List<string>();

        public List<string> DisableScenes { get => disableScenes; }
        #endregion

        #region Methods
        void Awake()
        {
            InstantiatePersistentObjectsPool();
        }

        private void InstantiatePersistentObjectsPool()
        {
            if (PersistentObjectsPool.persistentObjectsPool == null)
            {
                PersistentObjectsPool.persistentObjectsPool = new PersistentObjectsPool();
            }

            PersistentObjectsPool.persistentObjectsPool.InitializeInstance(this);
        }

        private void OnDestroy()
        {
            PersistentObjectsPool.persistentObjectsPool.RemoveObjectFromPool(this);
        }
        #endregion
    }
}