// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using ColdironTools.Events;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// Allows scene loading to be controlled by Unity Events and Game Events.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        #region Fields
        [Tooltip("The game event that will load the next scene when raised.")]
        [SerializeField] private GameEvent loadNextScene = null;

        [Tooltip("The game event that will load the previous scene when raised.")]
        [SerializeField] private GameEvent loadPreviousScene = null;

        [Tooltip("The game event that will reload the current scene when raised.")]
        [SerializeField] private GameEvent reloadCurrentScene = null;
        #endregion

        #region Methods
        /// <summary>
        /// Registers listeners.
        /// </summary>
        private void Awake()
        {
            loadNextScene?.RegisterAction(LoadNextScene);
            reloadCurrentScene?.RegisterAction(ReloadCurrentScene);
            loadPreviousScene?.RegisterAction(LoadPreviousScene);
        }

        /// <summary>
        /// Unregisteres listeners.
        /// </summary>
        private void OnDestroy()
        {
            loadNextScene?.UnregisterAction(LoadNextScene);
            reloadCurrentScene?.UnregisterAction(ReloadCurrentScene);
            loadPreviousScene?.UnregisterAction(LoadPreviousScene);
        }

        /// <summary>
        /// Loads scene by index.
        /// </summary>
        /// <param name="sceneIndex">Build index of scene to load.</param>
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        /// <summary>
        /// Loads scene by name.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Loads next scene in build index if it exists.
        /// </summary>
        public static void LoadNextScene()
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextScene <= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextScene);
            }
        }

        /// <summary>
        /// Loads next scene in build index if it exists.
        /// </summary>
        public static void ReloadCurrentScene()
        {
            int thisScene = SceneManager.GetActiveScene().buildIndex;
            if (thisScene <= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(thisScene);
            }
        }

        /// <summary>
        /// Loads previous scene in build index if it exists.
        /// </summary>
        public static void LoadPreviousScene()
        {
            int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
            if (previousScene >= 0)
            {
                SceneManager.LoadScene(previousScene);
            }
        }
        #endregion
    }
}