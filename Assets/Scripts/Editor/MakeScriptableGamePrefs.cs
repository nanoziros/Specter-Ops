﻿using UnityEngine;
using System.Collections;
using SpecterOps;
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

        gamePrefs.RewardPerCollectable = 1;
        gamePrefs.MinCollectablePerTile = 1;
        gamePrefs.MaxCollectablePerTile = 3;

        // Save .asset file
        AssetDatabase.CreateAsset(gamePrefs, GameDataPresenter.GamePrefsPath);
        AssetDatabase.SaveAssets();

        // Focus on new asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gamePrefs;
    }
}