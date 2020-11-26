using UnityEngine;
using UnityEngine.SceneManagement;
using ColdironTools.Events;

namespace ColdironTools.Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameEvent loadNextScene = null;
        [SerializeField] private GameEvent loadPreviousScene = null;

        private void Awake()
        {
            loadNextScene?.RegisterAction(LoadNextScene);
            loadPreviousScene?.RegisterAction(LoadPreviousScene);
        }

        private void OnDestroy()
        {
            loadNextScene?.UnregisterAction(LoadNextScene);
            loadPreviousScene?.UnregisterAction(LoadPreviousScene);
        }

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadNextScene()
        {
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextScene <= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextScene);
            }
        }

        public void LoadPreviousScene()
        {
            int previousScene = SceneManager.GetActiveScene().buildIndex - 1;
            if (previousScene >= 0)
            {
                SceneManager.LoadScene(previousScene);
            }
        }
    }
}