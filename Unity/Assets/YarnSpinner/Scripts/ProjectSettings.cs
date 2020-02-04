﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ProjectSettings : ScriptableObject {
    [SerializeField]
    private List<string> _textProjectLanguages = new List<string>();
    public static List<string> TextProjectLanguages => Instance._textProjectLanguages;

    [SerializeField]
    private List<string> _audioProjectLanguages = new List<string>();
    public static List<string> AudioProjectLanguages => Instance._audioProjectLanguages;

    /// <summary>
    /// The path to store the project settings
    /// </summary>
    private string _settingsPath;
    public static string SettingsPath => Instance._settingsPath;

    /// <summary>
    /// Instance of this class (Singleton design pattern)
    /// </summary>
    private static ProjectSettings _instance;

    /// <summary>
    /// Makes sure that there's always an instance of this 
    /// class alive upon access.
    /// </summary>
    private static ProjectSettings Instance {
        get {
            if (!_instance) {
                // Calls Awake() implicitly
                _instance = CreateInstance<ProjectSettings>();
            }
            return _instance;
        }
    }


    private void Awake() {
        if (_instance != null && this != _instance) {
            DestroyImmediate(_instance);
        }
        _instance = this;

#if UNITY_EDITOR
        _settingsPath = Application.dataPath + "/../ProjectSettings" + "/YarnProjectSettings.json";
        YarnSettingsHelper.ReadPreferencesFromDisk(this, _settingsPath, Initialize);
#endif
#if UNITY_PLAYER
        _settingsPath = "YarnProjectSettings.json"
        var jsonString = Resources.Load<TextAsset>(_settingsPath).ToString();
        if (!string.IsNullOrEmpty(jsonString)) {
            YarnSettingsHelper.ReadJsonFromString(this, jsonString, Initialize);
        }
#endif
        Debug.Log(_settingsPath);
    }

    private void OnDestroy() {
        YarnSettingsHelper.WritePreferencesToDisk(this, _settingsPath);
    }

    private void Initialize () {
        _textProjectLanguages = new List<string>();
        _audioProjectLanguages = new List<string>();
    }
}
