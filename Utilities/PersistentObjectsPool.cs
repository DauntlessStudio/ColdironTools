using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ColdironTools.Utilities
{
    public class PersistentObjectsPool
    {
        #region Fields
        private List<PersistentObject> persistentObjects = new List<PersistentObject>();

        //pool accessed by any persistant objects. Instantiated by the first persistant object to Awake()
        public static PersistentObjectsPool persistentObjectsPool;
        #endregion

        #region Methods
        public PersistentObjectsPool()
        {
            SceneManager.activeSceneChanged += OnNewScene;
        }

        private void OnNewScene(Scene arg1, Scene arg2)
        {
            foreach (PersistentObject persistentObject in persistentObjects)
            {
                persistentObject.gameObject.SetActive(persistentObject.DisableScenes.Contains(arg2.name) ? false : true);
            }
        }

        public void InitializeInstance(PersistentObject persistentObject)
        {
            if (FoundDuplicate(persistentObject))
            {
                DestroyDuplicate(persistentObject);
                return;
            }

            UnparentObject(persistentObject);

            AddObjectToPool(persistentObject);
        }

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

        private void DestroyDuplicate(PersistentObject persistentObject)
        {
            Object.Destroy(persistentObject.gameObject);
        }

        private void UnparentObject(PersistentObject persistentObject)
        {
            if (persistentObject.transform.parent) persistentObject.transform.SetParent(null);
        }

        private void AddObjectToPool(PersistentObject persistentObject)
        {
            persistentObjects.Add(persistentObject);
            Object.DontDestroyOnLoad(persistentObject.gameObject);
        }

        public void RemoveObjectFromPool(PersistentObject persistentObject)
        {
            persistentObjects.Remove(persistentObject);
        }
        #endregion
    }
}