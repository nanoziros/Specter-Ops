namespace SpecterOps
{
    using UnityEngine;
    using SpecterOps.Player;

    /// <summary>
    /// Main scripteable object manager. This class is persistent and its setup allow for its use both in Editor and in Build versions
    /// </summary>
    public class GameDataPresenter : MonoBehaviour
    {
        // Singleton
        protected static GameDataPresenter instance;
        public static GameDataPresenter Instance
        {
            get
            {
                return instance;
            }
            private set { instance = value; }
        }
        // Public game prefs path
        public static string GamePrefsFolder = "Assets/Resources/ScripteableObjects/";
        public static string GamePrefsName = "GamePreferences.asset";

        // Game data
        [System.NonSerialized]
        public GamePrefs GamePrefs; 
        public static GamePrefs DefaultGamePref 
        {
            get
            {
                return new GamePrefs
                {
                    MatchDuration = 30.0f,
                    EnvironmentSpeed = 3,
                    PlayerSpeed = 4.0f,
                    PlayerHealth = 3,
                    MinEnemyPerTile = 1,
                    MaxEnemyPerTile = 2,
                    EnemyCollisionDamage = 1,
                    EnemyProjectileDamage = 1,
                    EnemyProjectileSpeed = 5,
                    EnemyFireRate = 0.8f,
                    RewardPerCollectable = 1,
                    MinCollectablePerTile = 1,
                    MaxCollectablePerTile = 3,
                    inputConfig = new InputInstance.InputConfiguration
                    {
                        PauseRequest = KeyCode.Escape,
                        MoveForward = KeyCode.W,
                        MoveBack = KeyCode.S,
                        StrafeLeft = KeyCode.A,
                        StrafeRight = KeyCode.D,
                        AltMoveForward = KeyCode.UpArrow,
                        AltMoveBack = KeyCode.DownArrow,
                        AltStrafeLeft = KeyCode.LeftArrow,
                        AltStrafeRight = KeyCode.RightArrow
                    }
                };
            }
        }

        /// <summary>
        ///   Singleton and inter scene persistence setup
        /// </summary>
        /// <devdoc>
        ///     Destroys itself if there is already one in the scene.
        /// </devdoc>
        public void Awake()
        {
            if (GameDataPresenter.Instance != null)
            {
                Object.Destroy(this.gameObject);
                return;
            }
            
            // Set singleton and persistence setup
            GameDataPresenter.Instance = this;
            Object.DontDestroyOnLoad(this.gameObject);

            // Load data from serializable object
            // todo: execute this.LoadFromScripteableObject() instead of LoadFromScripteableObject() if this isn't the first time the game is ran (using playerpref)
            this.LoadFromScripteableObject();
        }

        /// <summary>
        /// Load the game preferences object from scripteable object
        /// </summary>
        public bool LoadFromScripteableObject()
        {
            // Since we're going to use resources.load, we can remove the Assets/Resources prefix
            string finalGamePrefsPath = GamePrefsFolder.Replace("Assets/Resources/", "");
            finalGamePrefsPath += GamePrefsName.Replace(".asset", "");

            // Attempt to load the game prefs .asset
            this.GamePrefs = Resources.Load(finalGamePrefsPath) as GamePrefs;
            if (this.GamePrefs == null)
            {
                Debug.LogError("Couldn't load " + finalGamePrefsPath);
                return false;
            }
            // Store game prefs in build prefs txt
            SaveLoad.SavePrefs(this.GamePrefs);
            this.GamePrefs = SaveLoad.LoadPrefs();

            // Return success
            return this.GamePrefs != null;
        }

        /// <summary>
        /// Load the game preferences object from gameplay .txt prefs
        /// </summary>
        public bool LoadFromGameplayFile()
        {
            // Load game prefs
            this.GamePrefs = SaveLoad.LoadPrefs();

            // Return success
            return this.GamePrefs != null;
        }

        /// <summary>
        /// Saves the game preferences object from gameplay .txt prefs
        /// </summary>
        public void SaveCurrentPrefsToGameplayFile()
        {
            // Store game prefs in build prefs txt
            SaveLoad.SavePrefs(this.GamePrefs);
        }

    }
}
