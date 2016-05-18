using UnityEngine;
using System.Collections;

/// <summary>
/// Scripteable object will all gameplay configurations
/// </summary>
[System.Serializable]
public class GamePrefs : ScriptableObject
{
    [Header("General Match Parameters")]
    [Range(30, 60)]
    public float MatchDuration;

    [Header("Environment Parameters")]
    public float EnvironmentSpeed;

    [Header("Player Parameters")]
    public float PlayerSpeed;
    public int PlayerHealth;

    [Header("Enemy Parameters")]
    [Range(0, 18)]
    public int MinEnemyPerTile;
    [Range(0, 18)]
    public int MaxEnemyPerTile;
    public int EnemyCollisionDamage;
    public int EnemyProjectileDamage;
    public float EnemyProjectileSpeed;

    [Header("Collectable Parameters")]
    public int RewardPerCollectable;
    [Range(0, 18)]
    public int MinCollectablePerTile;
    [Range(0, 18)]
    public int MaxCollectablePerTile;
}
