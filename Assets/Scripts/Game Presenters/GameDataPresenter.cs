namespace SpecterOps
{
    using UnityEngine;
    using SpecterOps.Player;

    /// <summary>
    /// Main scripteable object manager
    /// </summary>
    public class GameDataPresenter : MonoBehaviour
    {
        // Public game prefs path
        public static string GamePrefsFolder = "Assets/Resources/ScripteableObjects/";
        public static string GamePrefsName = "GamePreferences.asset";

        // Game data
        public GamePrefs GamePrefs { get; private set; }
        public static GamePrefs DefaultGamePref = new GamePrefs
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
        /// <summary>
        /// Load the game preferences object
        /// </summary>
        public bool Load()
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

            // Return success
            return true;
        }

    }
}
