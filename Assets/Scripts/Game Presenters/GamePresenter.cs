namespace SpecterOps
{
    using UnityEngine;
    using System.Collections;

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

        // Match parameters
        [Header("Match Time")]
        [Range(30,60)]
        public float MatchDuration = 30.0f;

        // Current match status
        public GameState CurrentMatchState = GameState.NonStarted;
        public float CurrentMatchDuration = 0.0f;

        // Enemy parameters
        [Header("Enemies Parameters")]
        public EnemyController EnemyPrefab;
        public int DamagePerCollision = 1;
        public int DamagePerProjectile = 1;
        public float ProjectileSpeed = 3;
        [Range(0, 18)]
        public int MinEnemyPerTile = 1;
        [Range(0, 18)]
        public int MaxEnemyPerTile = 9;

        // Collectable parameters
        [Header("Collectable Parameters")]
        public CollectableController CollectablePrefab;
        public int RewardPerCollectable = 1;
        [Range(0, 18)]
        public int MinCollectablePerTile = 1;
        [Range(0, 18)]
        public int MaxCollectablePerTile = 9;

        // Singleton
        private static GamePresenter _instance = null;
        public static GamePresenter Instance
        {
            get { return GamePresenter._instance; }
        }

        /// <summary>
        /// Executed at the start of the game (Exclusively for singletons in this design)
        /// </summary>
        void Awake()
        {
            // Set singleton refere
            GamePresenter._instance = this;
        }

        /// <summary>
        /// Use this for initialization (this is the only class that can use this method in GameScene)
        /// </summary>
        void Start()
        {
            // Get core presenters
            this.PlayerPresenter = this.GetComponentInChildren<PlayerPresenter>();
            this.EnvironmentPresenter = this.GetComponentInChildren<EnvironmentPresenter>();
            this.CameraRigPresenter = this.GetComponentInChildren<CameraRigPresenter>();
            this.ProjectilePresenter = this.GetComponentInChildren<ProjectilePresenter>();

            // Load gameplay information from scripteable object
            // todo:

            // Initialize core presenters
            this.ProjectilePresenter.Initialize();
            this.EnvironmentPresenter.Initialize();
            this.PlayerPresenter.Initialize(this.EnvironmentPresenter);

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
                // Calculate final score
                int finalScore = this.PlayerPresenter.Player.CurrentHealthPoints +
                                 this.PlayerPresenter.Player.CurrentCollectables;

                // todo:
                Debug.Log("You Lost! Your score is " + finalScore);

            }
            // Player ended the level
            else
            {
                // Calculate final score
                int finalScore = this.PlayerPresenter.Player.CurrentHealthPoints +
                                 this.PlayerPresenter.Player.CurrentCollectables;

                // todo:
                Debug.Log("You won! Your score is " + finalScore);
            }
        }

        /// <summary>
        /// Match timer updater
        /// </summary>
        IEnumerator UpdateMatchTimer()
        {
            // Execute timer
            while (this.CurrentMatchDuration < this.MatchDuration)
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
            Paused, //todo: implement methods
            Ended
        }

    }
}
