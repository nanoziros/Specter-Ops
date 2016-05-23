using System;

namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using UnityEditor;
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

        // UI Variables
        /*
        public int PlayerHealth
        {
            get
            {
                if(this.TempGamePrefs!= null)
                return this.TempGamePrefs.PlayerHealth;
            }
            set { this.TempGamePrefs}
        }
        */
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

            // Save new parameters
            // Create .asset file
            GamePrefs gamePrefs = ScriptableObject.CreateInstance<GamePrefs>();

            // Set its new values
            gamePrefs = this.TempGamePrefs;
            // Create parent folder
            if (!Directory.Exists(GameDataPresenter.GamePrefsFolder))
                Directory.CreateDirectory(GameDataPresenter.GamePrefsFolder);

            // Save .asset file
            GamePrefs asset =
                AssetDatabase.LoadAssetAtPath<GamePrefs>(GameDataPresenter.GamePrefsFolder + GameDataPresenter.GamePrefsName);
            if (asset == null)
                return;
            asset = gamePrefs;
            AssetDatabase.Refresh(ImportAssetOptions.Default);
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