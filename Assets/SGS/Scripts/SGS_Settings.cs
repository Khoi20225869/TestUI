//----------------------------------------------
//          Simple Garage System
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shared settings of the SGS.
/// </summary>
public class SGS_Settings : ScriptableObject {

    private static SGS_Settings instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static SGS_Settings Instance { get { if (instance == null) instance = Resources.Load("SGS_Settings") as SGS_Settings; return instance; } }

    /// <summary>
    /// Main menu scene index in the build settings.
    /// </summary>
    [Min(0)] public int mainMenuSceneIndex = 0;

    /// <summary>
    /// Default money.
    /// </summary>
    [Header("Default Settings")] [Min(0)] public int defaultMoney = 10000;

    /// <summary>
    /// Default selected vehicle index.
    /// </summary>
    [Min(0)] public int defaultSelectedVehicleIndex = 0;

    /// <summary>
    /// Default audio volume.
    /// </summary>
    [Range(0f, 1f)] public float defaultAudioVolume = 1f;

    /// <summary>
    /// Default music volume.
    /// </summary>
    [Range(0f, 1f)] public float defaultMusicVolume = .65f;

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    [Header("PlayerPrefs Save/Load Keys")] public string playerPrefsLastSelectedVehicleIndex = "LastSelectedVehicleIndex";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsPlayerMoney = "Money";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsPlayerVehicle = "Vehicle";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsPlayerName = "PlayerName";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsSelectedScene = "Scene";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsAudioVolume = "AudioVolume";

    /// <summary>
    /// String used to save and load via PlayerPrefs.
    /// </summary>
    public string playerPrefsMusicVolume = "MusicVolume";

    /// <summary>
    /// Main menu resources.
    /// </summary>
    public GameObject mainMenuResources;

    /// <summary>
    /// Gameplay resources.
    /// </summary>
    public GameObject gameplayResources;

}
