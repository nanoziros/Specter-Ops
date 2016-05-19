using System;

namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Game UI Hub. Manages the Player Status View, Match Status View and th Victory/Defeat/Pause Views
    /// </summary>
    public class GameUIPresenter : MonoBehaviour
    {
        // Control parameters
        private bool isInitialized = false;

        // Timer cosmetic variables
        public Color FarTimeColor = Color.red;
        public Color CloseTimeColor = Color.green;

        #region Match model data providers
        public string CurrentNormalizedHealth
        {
            get
            {
                // Get max health
                float maxHealth = (GamePresenter.Instance.DataPresenter != null &&
                                   GamePresenter.Instance.GamePrefs != null)
                    ? GamePresenter.Instance.GamePrefs.PlayerHealth
                    : 1;
                // Get current health
                float currentHealth = (GamePresenter.Instance.PlayerPresenter != null &&
                                       GamePresenter.Instance.PlayerPresenter.Player != null)
                    ? GamePresenter.Instance.PlayerPresenter.Player.CurrentHealthPoints
                    : 0;
                // Calculate and format normalized health
                return (1.0f - currentHealth/maxHealth).ToString();
            }
        }
        public Color CurrentMatchDurationColor
        {
            get
            {
                // Get max match duration
                float matchMaxDuration = (GamePresenter.Instance.DataPresenter != null &&
                                          GamePresenter.Instance.GamePrefs != null)
                    ? GamePresenter.Instance.GamePrefs.MatchDuration
                    : 1;
                // Get current normalized duration
                float normalizedDuration = GamePresenter.Instance.CurrentMatchDuration/matchMaxDuration;
                // Calculate proper color
                return Color.Lerp(this.FarTimeColor, this.CloseTimeColor, normalizedDuration);
            }
        }
        public string CurrentMatchDuration
        {
            get
            {
                // Get max match duration
                float matchMaxDuration = (GamePresenter.Instance.DataPresenter != null &&
                                          GamePresenter.Instance.GamePrefs != null)
                    ? GamePresenter.Instance.GamePrefs.MatchDuration
                    : 0;
                // Calculate and format remaining match time
                return Mathf.CeilToInt(matchMaxDuration - GamePresenter.Instance.CurrentMatchDuration).ToString();
            }
        }
        public string CurrentCollectables
        {
            get
            {
                // Calculate and format remaining match time
                int currentCollectables = (GamePresenter.Instance.PlayerPresenter != null &&
                                           GamePresenter.Instance.PlayerPresenter.Player != null)
                    ? GamePresenter.Instance.PlayerPresenter.Player.CurrentCollectables
                    : 0;
                return currentCollectables.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Use this method to initialize the UI
        /// </summary>
        public void Initialize()
        {
            // Initialize screen views
            // todo:

            // Subscribe to end game events
            GamePresenter.Instance.MatchEnded += this.DisplayMatchEndScreen;

            // Mark control flags
            this.isInitialized = true;
        }

        /// <summary>
        /// Since we did some event subscribing, we need to safely unsubscribe on destroy (to avoid nullreference errors)
        /// </summary>
        public void OnDestroy()
        {
            // Check if we initialized this class
            if (!this.isInitialized)
                return;

            // Unsubscribe collision events
            GamePresenter.Instance.MatchEnded -= this.DisplayMatchEndScreen;
        }

        /// <summary>
        /// Display the end game screen matching the game results
        /// </summary>
        public void DisplayMatchEndScreen(GamePresenter.GameResult result, int score)
        {
            switch (result)
            {
                case GamePresenter.GameResult.Win:
                    // todo:
                    break;
                case GamePresenter.GameResult.Lose:
                    // todo:
                    break;
            }
        }
    }
}