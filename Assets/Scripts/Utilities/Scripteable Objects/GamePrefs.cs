using UnityEngine;
using SpecterOps.Player;

/// <summary>
/// Data structure for game preferences, this is required for the ingame preferences configuration panel
/// </summary>
[System.Serializable]
public class GamePrefsFile
{
    public float MatchDuration;
    public float EnvironmentSpeed;
    public float PlayerSpeed;
    public int PlayerHealth;
    public int MinEnemyPerTile;
    public int MaxEnemyPerTile;
    public int EnemyCollisionDamage;
    public int EnemyProjectileDamage;
    public float EnemyProjectileSpeed;
    public float EnemyFireRate;
    public int RewardPerCollectable;
    public int MinCollectablePerTile;
    public int MaxCollectablePerTile;
    public InputInstance.InputConfiguration inputConfig;
}

/// <summary>
/// Scripteable object will all gameplay configurations
/// </summary>
[System.Serializable]
public class GamePrefs : ScriptableObject
{
    [Header("General Match")]
    [Range(30, 60)]
    public float MatchDuration;

    [Header("Environment")]
    [Tooltip("To avoid the appareance of slower projectiles, this value must be LOWER than EnemyProjectileSpeed")]
    [Range(0.1f,100.0f)]
    public float EnvironmentSpeed;

    [Header("Player")]
    [Range(0.1f, 100.0f)]
    public float PlayerSpeed;
    public int PlayerHealth;

    [Header("Enemy")]
    [Range(0, 18)]
    public int MinEnemyPerTile;
    [Range(0, 18)]
    public int MaxEnemyPerTile;
    public int EnemyCollisionDamage;
    public int EnemyProjectileDamage;
    [Tooltip("To avoid the appareance of slower projectiles, this value must be HIGHER than EnvironmentSpeed")]
    [Range(0.1f, 100.0f)]
    public float EnemyProjectileSpeed;
    [Range(0.1f, 100.0f)]
    public float EnemyFireRate;

    [Header("Collectable")]
    public int RewardPerCollectable;
    [Range(0, 18)]
    public int MinCollectablePerTile;
    [Range(0, 18)]
    public int MaxCollectablePerTile;

    [Header("Player Input")]
    public InputInstance.InputConfiguration inputConfig;
}
