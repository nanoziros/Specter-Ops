namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections;
    using System.IO;
    using System;

#if UNITY_EDITOR
    using UnityEditor;
#endif
    /// <summary>
    /// This UI can change the gameplay preferences
    /// </summary>
    public class SettingsPresenter : MonoBehaviour
    {
        // Game preferences
        [System.NonSerialized]
        public GamePrefs TempGamePrefs;

        /// <summary>
        /// Load current gameplay configuration
        /// </summary>
        private void OnEnable()
        {
            // Load data
            this.TempGamePrefs = GameDataPresenter.Instance.GamePrefs;
        }

        // We only execute scripteable object update in Editor
        /// <summary>
        /// Store gameplay changes
        /// </summary>
        private void OnDisable()
        {
            this.SavePreferences();
        }

        /// <summary>
        /// This method saves the current preferences on disk
        /// </summary>
        private void SavePreferences()
        {
            // Save .asset file
#if UNITY_EDITOR
            AssetDatabase.Refresh(ImportAssetOptions.Default);
#endif
            SaveLoad.SavePrefs(this.TempGamePrefs);
        }

        /// <summary>
        /// Request config reset
        /// </summary>
        public void RequestReset()
        {
            // Don't execute if the load wasn't succesful
            if (GameDataPresenter.Instance.GamePrefs == null)
                return;

            // Reset preferences
            this.TempGamePrefs.MatchDuration         = GameDataPresenter.DefaultGamePref.MatchDuration;
            this.TempGamePrefs.PlayerSpeed           = GameDataPresenter.DefaultGamePref.PlayerSpeed;
            this.TempGamePrefs.PlayerHealth          = GameDataPresenter.DefaultGamePref.PlayerHealth;
            this.TempGamePrefs.MinEnemyPerTile       = GameDataPresenter.DefaultGamePref.MinEnemyPerTile;
            this.TempGamePrefs.MaxEnemyPerTile       = GameDataPresenter.DefaultGamePref.MaxEnemyPerTile;
            this.TempGamePrefs.EnemyCollisionDamage  = GameDataPresenter.DefaultGamePref.EnemyCollisionDamage;
            this.TempGamePrefs.EnemyProjectileDamage = GameDataPresenter.DefaultGamePref.EnemyProjectileDamage;

            // Save .asset file
            this.SavePreferences();
        }

#region UI Variables
        public string MatchDurationString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.MatchDuration.ToString();
            }
        }
        public float MatchDuration
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.MatchDuration;
            }
            set
            {
                if (this.TempGamePrefs != null)
                    this.TempGamePrefs.MatchDuration = value;
            }
        }

        public string PlayerSpeedString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.PlayerSpeed.ToString();
            }
        }
        public float PlayerSpeed
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.PlayerSpeed;
            }
            set
            {
                if (this.TempGamePrefs != null)
                    this.TempGamePrefs.PlayerSpeed = value;
            }
        }

        public string PlayerHealthString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.PlayerHealth.ToString();
            }
        }
        public float PlayerHealth
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.PlayerHealth;
            }
            set
            {
                if (this.TempGamePrefs != null)
                    this.TempGamePrefs.PlayerHealth = Mathf.CeilToInt(value);
            }
        }

        public string MinEnemyPerTileString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.MinEnemyPerTile.ToString();
            }
        }
        public float MinEnemyPerTile
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.MinEnemyPerTile;
            }
            set
            {
                // note: we must make sure that the min enemies per tile is higher than the max enemies per tile value
                if (this.TempGamePrefs != null && value <= this.TempGamePrefs.MaxEnemyPerTile)
                    this.TempGamePrefs.MinEnemyPerTile = Mathf.CeilToInt(value);
            }
        }

        public string MaxEnemyPerTileString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.MaxEnemyPerTile.ToString();
            }
        }
        public float MaxEnemyPerTile
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.MaxEnemyPerTile;
            }
            set
            {
                // note: we must make sure that the min enemies per tile is higher than the max enemies per tile value
                if (this.TempGamePrefs != null && value >= this.TempGamePrefs.MinEnemyPerTile)
                    this.TempGamePrefs.MaxEnemyPerTile = Mathf.CeilToInt(value);
            }
        }

        public string EnemyCollisionDamageString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.EnemyCollisionDamage.ToString();
            }
        }
        public float EnemyCollisionDamage
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.EnemyCollisionDamage;
            }
            set
            {
                if (this.TempGamePrefs != null)
                    this.TempGamePrefs.EnemyCollisionDamage = Mathf.CeilToInt(value);
            }
        }

        public string EnemyProjectileDamageString
        {
            get
            {
                return this.TempGamePrefs == null ? "0" : this.TempGamePrefs.EnemyProjectileDamage.ToString();
            }
        }
        public float EnemyProjectileDamage
        {
            get
            {
                return this.TempGamePrefs == null ? 0 : this.TempGamePrefs.EnemyProjectileDamage;
            }
            set
            {
                if (this.TempGamePrefs != null)
                    this.TempGamePrefs.EnemyProjectileDamage = Mathf.CeilToInt(value);
            }
        }


    }
    #endregion
}