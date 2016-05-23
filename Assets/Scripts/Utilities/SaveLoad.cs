using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{

    public static void SavePrefs(GamePrefs prefs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerPrefs.txt");
        bf.Serialize(file, GamePrefToGamePrefFile(prefs));
        file.Close();
    }

    public static GamePrefs LoadPrefs()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerPrefs.txt"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerPrefs.txt", FileMode.Open);
            GamePrefsFile prefs = (GamePrefsFile)bf.Deserialize(file);
            file.Close();

            return GamePrefFileToGamePref(prefs);
        }
        return null;
    }

    public static GamePrefs GamePrefFileToGamePref(GamePrefsFile file)
    {
        return new GamePrefs
        {
            MatchDuration = file.MatchDuration,
            EnvironmentSpeed = file.EnvironmentSpeed,
            PlayerSpeed = file.PlayerSpeed,
            PlayerHealth = file.PlayerHealth,
            MinEnemyPerTile = file.MinEnemyPerTile,
            MaxEnemyPerTile = file.MaxEnemyPerTile,
            EnemyCollisionDamage = file.EnemyCollisionDamage,
            EnemyProjectileDamage = file.EnemyProjectileDamage,
            EnemyProjectileSpeed = file.EnemyProjectileSpeed,
            EnemyFireRate = file.EnemyFireRate,
            RewardPerCollectable = file.RewardPerCollectable,
            MinCollectablePerTile = file.MinCollectablePerTile,
            MaxCollectablePerTile = file.MaxEnemyPerTile,
            inputConfig = file.inputConfig
        };
    }

    public static GamePrefsFile GamePrefToGamePrefFile(GamePrefs gamePrefs)
    {
        return new GamePrefsFile
        {
            MatchDuration = gamePrefs.MatchDuration,
            EnvironmentSpeed = gamePrefs.EnvironmentSpeed,
            PlayerSpeed = gamePrefs.PlayerSpeed,
            PlayerHealth = gamePrefs.PlayerHealth,
            MinEnemyPerTile = gamePrefs.MinEnemyPerTile,
            MaxEnemyPerTile = gamePrefs.MaxEnemyPerTile,
            EnemyCollisionDamage = gamePrefs.EnemyCollisionDamage,
            EnemyProjectileDamage = gamePrefs.EnemyProjectileDamage,
            EnemyProjectileSpeed = gamePrefs.EnemyProjectileSpeed,
            EnemyFireRate = gamePrefs.EnemyFireRate,
            RewardPerCollectable = gamePrefs.RewardPerCollectable,
            MinCollectablePerTile = gamePrefs.MinCollectablePerTile,
            MaxCollectablePerTile = gamePrefs.MaxEnemyPerTile,
            inputConfig = gamePrefs.inputConfig
        };
    }


}
