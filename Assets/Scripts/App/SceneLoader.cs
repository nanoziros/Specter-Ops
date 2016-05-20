using UnityEngine.SceneManagement;

namespace SpecterOps.App
{
    using UnityEngine;

    /// <summary>
    /// This class manages scene loading
    /// </summary>
    public class SceneLoader : MonoBehaviour {
        /// <summary>
        /// Load the requested scene
        /// </summary>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Close the application
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
