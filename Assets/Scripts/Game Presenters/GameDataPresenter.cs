namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Main scripteable object manager
    /// </summary>
    public class GameDataPresenter : MonoBehaviour
    {
        // Public game prefs path
        public static string GamePrefsPath = "Assets/Resources/ScripteableObjects/GamePreferences.asset";

        // Game data
        public GamePrefs GamePrefs;

        /// <summary>
        /// Load the game preferences object
        /// </summary>
        public bool Load()
        {
            // Since we're going to use resources.load, we can remove the Assets/Resources prefix
            string finalGamePrefsPath = GamePrefsPath.Replace("Assets/Resources/", "");
            finalGamePrefsPath = finalGamePrefsPath.Replace(".asset", "");

            // Attempt to load the game prefs .asset
            this.GamePrefs = Resources.Load(finalGamePrefsPath) as GamePrefs;
            if (this.GamePrefs == null)
            {
                Debug.LogError("Couldn't load " + finalGamePrefsPath);
                return false;
            }

            // Return success
            return true;
        }

    }
}
