using UnityEngine;
using System.Collections;
using SpecterOps.Player;

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
    [Tooltip("To avoid the appareance of slower projectiles, this value must be lower than EnemyProjectileSpeed")]
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
    [Tooltip("To avoid the appareance of slower projectiles, this value must be higher than EnvironmentSpeed")]
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
