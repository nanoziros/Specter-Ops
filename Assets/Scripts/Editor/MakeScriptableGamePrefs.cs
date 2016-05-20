using UnityEngine;
using System.IO;
using SpecterOps;
using SpecterOps.Player;
using UnityEditor;

public class MakeScriptableGamePrefs
{
    /// <summary>
    /// Game prefs creator and resetter method
    /// </summary>
    [MenuItem("Tools/Game Preferences/Reset Game Preferences")]
    public static void ResetScriptableGamePrefs()
    {
        // Create .asset file
        GamePrefs gamePrefs = ScriptableObject.CreateInstance<GamePrefs>();

        // Set its default values
        gamePrefs.MatchDuration = 30.0f;

        gamePrefs.EnvironmentSpeed = 3;

        gamePrefs.PlayerSpeed = 4.0f;
        gamePrefs.PlayerHealth = 3;

        gamePrefs.MinEnemyPerTile = 1;
        gamePrefs.MaxEnemyPerTile = 2;
        gamePrefs.EnemyCollisionDamage = 2;
        gamePrefs.EnemyProjectileDamage = 1;
        gamePrefs.EnemyProjectileSpeed = 5;
        gamePrefs.EnemyFireRate = 1;

        gamePrefs.RewardPerCollectable = 1;
        gamePrefs.MinCollectablePerTile = 1;
        gamePrefs.MaxCollectablePerTile = 3;

        gamePrefs.inputConfig = new InputInstance.InputConfiguration
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
        };

        // Create parent folder
        if(!Directory.Exists(GameDataPresenter.GamePrefsFolder))
            Directory.CreateDirectory(GameDataPresenter.GamePrefsFolder);
        
        // Save .asset file
        AssetDatabase.CreateAsset(gamePrefs, GameDataPresenter.GamePrefsFolder + GameDataPresenter.GamePrefsName);
        AssetDatabase.SaveAssets();

        // Focus on new asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gamePrefs;
    }
}