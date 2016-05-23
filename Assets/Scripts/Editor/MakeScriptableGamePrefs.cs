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

        // Create .asset file and Set its default values
        var gamePrefs = GameDataPresenter.DefaultGamePref;
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