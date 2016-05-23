namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// This UI can change the gameplay preferences
    /// </summary>
    [RequireComponent(typeof(GameDataPresenter))]
    public class SettingsPresenter : MonoBehaviour
    {
        // Core components
        public GameDataPresenter DataPresenter;

        // Game preferences
        [System.NonSerialized]
        public GamePrefs TempGamePrefs;

        // UI Components

        // Control variables
        private bool loadSuccesful;


        /// <summary>
        /// Load current gameplay configuration
        /// </summary>
        private void OnEnable()
        {
            // Load data
            this.loadSuccesful = this.DataPresenter.Load();
            if (this.loadSuccesful)
                this.TempGamePrefs = this.DataPresenter.GamePrefs;
        }


        /// <summary>
        /// Store gameplay changes
        /// </summary>
        private void OnDisable()
        {
            // Don't execute if the load wasn't succesful
            if (!this.loadSuccesful)
                return;

        }

        /// <summary>
        /// Request config reset
        /// </summary>
        public void RequestReset()
        {
            // Don't execute if the load wasn't succesful
            if (!this.loadSuccesful)
                return;

            // Reset preferences
            this.TempGamePrefs = GameDataPresenter.DefaultGamePref;
        }
    }
}