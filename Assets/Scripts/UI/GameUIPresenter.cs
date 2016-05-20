using DG.Tweening;

namespace SpecterOps
{
    using UnityEngine;

    /// <summary>
    /// Game UI Hub. Manages the Player Status View, Match Status View and th Victory/Defeat/Pause Views
    /// </summary>
    public class GameUIPresenter : MonoBehaviour
    {
        // Control parameters
        private bool isInitialized = false;

        // Screen views
        public GameObject DefeatScreen;
        public GameObject VictoryScreen;

        // Generic animations
        public Ease PopUpAnimationEase = Ease.InBounce;
        public float PopUpAnimationLength = 0.5f;

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
                // Calculate and format current player collectables
                int currentCollectables = (GamePresenter.Instance.PlayerPresenter != null &&
                                           GamePresenter.Instance.PlayerPresenter.Player != null)
                    ? GamePresenter.Instance.PlayerPresenter.Player.CurrentCollectables
                    : 0;
                return currentCollectables.ToString();
            }
        }
        public string FinalPlayerScore
        {
            get
            {
                // Get final player score
                int playerScore = (GamePresenter.Instance.PlayerPresenter != null &&
                                           GamePresenter.Instance.PlayerPresenter.Player != null)
                    ? GamePresenter.Instance.PlayerPresenter.Player.PlayerScore
                    : 0;
                return playerScore.ToString();
            }
        }
        #endregion

        /// <summary>
        /// Use this method to initialize the UI
        /// </summary>
        public void Initialize()
        {
            // Initialize screen views
            this.DefeatScreen.SetActive(false);
            this.VictoryScreen.SetActive(false);

            // Subscribe to end game events
            GamePresenter.Instance.MatchEnded += this.DisplayMatchEndScreen;

            // Mark control flags
            this.isInitialized = true;

            // Hide cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
            // Display cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Display end game screen based on the game results
            switch (result)
            {
                case GamePresenter.GameResult.Win:
                    // Activate view
                    this.VictoryScreen.SetActive(true);
                    // Pop up animation
                    this.VictoryScreen.transform.localScale = Vector3.one*0.5f;
                    this.VictoryScreen.transform.DOScale(1.0f,this.PopUpAnimationLength).SetEase(this.PopUpAnimationEase);
                    break;
                case GamePresenter.GameResult.Lose:
                    // Activate view
                    this.DefeatScreen.SetActive(true);
                    // Pop up animation
                    this.DefeatScreen.transform.localScale = Vector3.one * 0.5f;
                    this.DefeatScreen.transform.DOScale(1.0f, this.PopUpAnimationLength).SetEase(this.PopUpAnimationEase);
                    break;
            }
        }
    }
}