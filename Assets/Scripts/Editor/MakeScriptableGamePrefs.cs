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
        gamePrefs = GameDataPresenter.DefaultGamePref;

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