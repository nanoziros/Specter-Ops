namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;
    using System;

    /// <summary>
    /// This class is the core game manager during gameplay, it starts and ends a game session.
    /// Aditionally this class handles final scoring and timer
    /// </summary>
    [RequireComponent(typeof(PlayerPresenter))]
    public class GamePresenter : MonoBehaviour
    {
        // Core presenters
        public PlayerPresenter PlayerPresenter { get; private set; }
        public EnvironmentPresenter EnvironmentPresenter { get; private set; }
        public CameraRigPresenter CameraRigPresenter { get; private set; }
        public ProjectilePresenter ProjectilePresenter { get; private set; }
        public VFXPresenter VfxPresenter { get; private set; }
        public GameDataPresenter DataPresenter { get; private set; }
        public GameUIPresenter UiPresenter { get; private set; }
        public GameSceneAudioPresenter AudioPresenter { get; private set; }

        // Match preferences and variable gameplay configuration
        public GamePrefs GamePrefs { get {return this.DataPresenter.GamePrefs; } }

        // Match parameters
        [Header("Match Parameters")]
        // Current match status
        public GameState CurrentMatchState = GameState.NonStarted;
        public float CurrentMatchDuration = 0.0f;

        // Enemy parameters
        [Header("Enemies Parameters")]
        public EnemyController EnemyPrefab;

        // Collectable parameters
        [Header("Collectable Parameters")]
        public CollectableController CollectablePrefab;

        // Game Events
        public event Action<GameResult,int> MatchEnded;
        public event Action GamePaused;
        public event Action GameResumed;

        // Singleton
        private static GamePresenter _instance = null;
        public static GamePresenter Instance
        {
            get
            {
                if (GamePresenter._instance == null)
                    GamePresenter._instance = (GamePresenter) FindObjectOfType(typeof (GamePresenter));
                return GamePresenter._instance;
            }
        }

        /// <summary>
        /// Use this for initialization (this is the only class that can use this method in GameScene with the exception of VideoLooper)
        /// </summary>
        void Start()
        {
            // Get core presenters
            this.DataPresenter = this.GetComponentInChildren<GameDataPresenter>();
            this.PlayerPresenter = this.GetComponentInChildren<PlayerPresenter>();
            this.EnvironmentPresenter = this.GetComponentInChildren<EnvironmentPresenter>();
            this.CameraRigPresenter = this.GetComponentInChildren<CameraRigPresenter>();
            this.ProjectilePresenter = this.GetComponentInChildren<ProjectilePresenter>();
            this.VfxPresenter = this.GetComponentInChildren<VFXPresenter>();
            this.UiPresenter = this.GetComponentInChildren<GameUIPresenter>();
            this.AudioPresenter = this.GetComponentInChildren<GameSceneAudioPresenter>();

            // Load gameplay information from scripteable object
            if (!this.DataPresenter.Load())
                return;
            
            // Initialize core presenters
            this.VfxPresenter.Initialize();
            this.ProjectilePresenter.Initialize();
            this.EnvironmentPresenter.Initialize();
            this.PlayerPresenter.Initialize(this.EnvironmentPresenter);
            this.UiPresenter.Initialize();
            this.AudioPresenter.Initialize();

            // Start game
            this.CurrentMatchState = GameState.Running;

            // Start environment animation
            this.EnvironmentPresenter.StartEnvironmentAnimation();

            // Start game timer
            StartCoroutine("UpdateMatchTimer");
        }

        /// <summary>
        /// Update all game elements
        /// </summary>
        void Update()
        {
            // Update camera
            if (GamePresenter.Instance.CurrentMatchState != GamePresenter.GameState.NonStarted)
                this.CameraRigPresenter.UpdateMainCamera();

            // Update player input
            this.PlayerPresenter.Player.UpdatePlayerInput();

            // Don't execute gameplay updates if the game isn't running
            if (GamePresenter.Instance.CurrentMatchState != GamePresenter.GameState.Running)
                return;

            // Update player
            this.PlayerPresenter.Player.UpdatePlayer();

            // Update environment tiles
            this.EnvironmentPresenter.UpdateEnvironment();

            // Update projectiles
            this.ProjectilePresenter.UpdateProjectiles();

            // Check if the player has died and, if so, end the match
            if(this.PlayerPresenter.Player.CurrentHealthPoints <= 0)
                this.EndMatch();
        }

        /// <summary>
        /// Update all game elements that require physics interval updates
        /// </summary>
        void FixedUpdate()
        {
            // Don't execute gameplay updates if the game isn't running
            if (GamePresenter.Instance.CurrentMatchState != GamePresenter.GameState.Running)
                return;

            // Update player
            this.PlayerPresenter.Player.FixedPlayerUpdate();
        }

        /// <summary>
        /// Ends the current match
        /// </summary>
        public void EndMatch()
        {
            // Set end game state
            this.CurrentMatchState = GameState.Ended;

            // we must check if the game ended because of completion or because the player died
            // Player died and therefore lost (ignore his score)
            if (this.PlayerPresenter.Player.CurrentHealthPoints <= 0)
            {
                // Raise defeat event
                Action<GameResult,int> handler = this.MatchEnded;
                if (handler != null) { handler(GameResult.Lose,this.PlayerPresenter.Player.PlayerScore); }

                // Debug
                //Debug.Log("You Lost! Your score is " + this.PlayerPresenter.Player.PlayerScore);

            }
            // Player ended the level so he/she won
            else
            {
                // Raise victory event
                Action<GameResult, int> handler = this.MatchEnded;
                if (handler != null) { handler(GameResult.Win, this.PlayerPresenter.Player.PlayerScore); }

                // Debug
                //Debug.Log("You won! Your score is " + this.PlayerPresenter.Player.PlayerScore);
            }
        }

        /// <summary>
        /// Use this method to request game pause or unpause (if the method is called when paused it will unpause the game and viceversa)
        /// </summary>
        public void ManagePauseGame()
        {
            // Pause game
            if (this.CurrentMatchState == GameState.Running)
            {
                // Raise pause event
                Action handler = this.GamePaused;
                if (handler != null) { handler(); }

                // Set status state
                this.CurrentMatchState = GameState.Paused;
            }
            // Unpause game
            else if (this.CurrentMatchState == GameState.Paused)
            {   
                // Raise unpause event
                Action handler = this.GameResumed;
                if (handler != null) { handler(); }

                // Set status state
                this.CurrentMatchState = GameState.Running;
            }
        }

        /// <summary>
        /// Match timer updater
        /// </summary>
        IEnumerator UpdateMatchTimer()
        {
            // Execute timer
            while (this.CurrentMatchDuration < this.GamePrefs.MatchDuration)
            {
                // Only update timer if the game is running
                if(this.CurrentMatchState == GameState.Running)
                    this.CurrentMatchDuration += Time.deltaTime;

                yield return null;
            }
            // Request match end screen
            this.EndMatch();
        }

        // Possible Game States
        public enum GameState
        {
            NonStarted,
            Running,
            Paused, 
            Ended
        }

        // Possible Game Results
        public enum GameResult
        {
            Win,
            Lose
        }

    }
}
