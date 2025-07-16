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
using UnityEngine.SceneManagement;

#if BCG_RCCP

/// <summary>
/// General API class.
/// </summary>
public class SGS {

    /// <summary>
    /// Gets the money.
    /// </summary>
    /// <returns></returns>
    public static int GetMoney() {

        return PlayerPrefs.GetInt(SGS_Settings.Instance.playerPrefsPlayerMoney, SGS_Settings.Instance.defaultMoney);

    }

    /// <summary>
    /// Changes the player money. It can be positive or negative.
    /// </summary>
    /// <param name="amount"></param>
    public static void ChangeMoney(int amount) {

        PlayerPrefs.SetInt(SGS_Settings.Instance.playerPrefsPlayerMoney, GetMoney() + amount);

    }

    /// <summary>
    /// Gets the latest selected player vehicle.
    /// </summary>
    /// <returns></returns>
    public static int GetVehicle() {

        UnlockVehicle(SGS_Settings.Instance.defaultSelectedVehicleIndex);
        int index = PlayerPrefs.GetInt(SGS_Settings.Instance.playerPrefsPlayerVehicle, SGS_Settings.Instance.defaultSelectedVehicleIndex);

        if (HasVehicle(index)) {

            return index;

        } else {

            PlayerPrefs.SetInt(SGS_Settings.Instance.playerPrefsPlayerVehicle, SGS_Settings.Instance.defaultSelectedVehicleIndex);
            return SGS_Settings.Instance.defaultSelectedVehicleIndex;

        }

    }

    /// <summary>
    /// Sets the selected vehicle as player vehicle.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void SetVehicle(int vehicleIndex) {

        PlayerPrefs.SetInt(SGS_Settings.Instance.playerPrefsPlayerVehicle, vehicleIndex);

    }

    public static bool HasVehicle(int vehicleIndex) {

        if (SGS_PlayerVehicles.Instance.playerVehicles.Length == 0) {

            Debug.LogError("SGS_PlayerVehicles doesn't have any vehicles yet, check your SGS_PlayerVehicles.");
            return false;

        }

        if (vehicleIndex >= SGS_PlayerVehicles.Instance.playerVehicles.Length) {

            Debug.LogError("Vehicle index is out of bounds, check your SGS_PlayerVehicles. Resetting the playerprefs key for the selected vehicle.");
            PlayerPrefs.SetInt(SGS_Settings.Instance.playerPrefsPlayerVehicle, SGS_Settings.Instance.defaultSelectedVehicleIndex);
            return false;

        }

        RCCP_CarController vehicle = SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle;

        if (!vehicle) {

            Debug.LogError("RCCP_CarController couldn't found on this vehicle, check your SGS_PlayerVehicles.");
            return false;

        }

        SGS_Player player = SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle.GetComponent<SGS_Player>();

        if (!player) {

            Debug.LogError("SGS_Player component couldn't found on this vehicle, adding it. Check your vehicle prefabs, they should have SGS_Player script.");
            SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle.gameObject.AddComponent<SGS_Player>();

        }

        return true;

    }

    /// <summary>
    ///  Unlocks the target vehicle.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void UnlockVehicle(int vehicleIndex) {

        if (HasVehicle(vehicleIndex))
            PlayerPrefs.SetInt(SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle.GetComponent<SGS_Player>().vehicleSaveName, 1);

    }

    /// <summary>
    /// Deleting save key for the target vehicle and locks it.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void LockVehicle(int vehicleIndex) {

        if (HasVehicle(vehicleIndex))
            PlayerPrefs.DeleteKey(SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle.GetComponent<SGS_Player>().vehicleSaveName);

    }

    /// <summary>
    /// Purchases and unlocks all vehicles.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void UnlockAllVehicles() {

        for (int i = 0; i < SGS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            if (SGS_PlayerVehicles.Instance.playerVehicles[i] != null)
                UnlockVehicle(i);

        }

    }

    /// <summary>
    /// Deleting save key for all vehicles and locking them.
    /// </summary>
    /// <param name="vehicleIndex"></param>
    public static void LockAllVehicles() {

        for (int i = 0; i < SGS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            if (SGS_PlayerVehicles.Instance.playerVehicles[i] != null)
                LockVehicle(i);

        }

    }

    /// <summary>
    /// Is this vehicle owned by the player?
    /// </summary>
    /// <param name="vehicleIndex"></param>
    /// <returns></returns>
    public static bool IsOwnedVehicle(int vehicleIndex) {

        if (!HasVehicle(vehicleIndex))
            return false;

        if (PlayerPrefs.HasKey(SGS_PlayerVehicles.Instance.playerVehicles[vehicleIndex].vehicle.GetComponent<SGS_Player>().vehicleSaveName))
            return true;
        else
            return false;

    }

    /// <summary>
    /// Sets the target scene to load.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public static void SetScene(int sceneIndex) {

        PlayerPrefs.SetInt(SGS_Settings.Instance.playerPrefsSelectedScene, sceneIndex);

    }

    /// <summary>
    /// Gets the latest selected scene.
    /// </summary>
    /// <returns></returns>
    public static int GetScene() {

        return PlayerPrefs.GetInt(SGS_Settings.Instance.playerPrefsSelectedScene, SGS_Settings.Instance.mainMenuSceneIndex);

    }

    /// <summary>
    /// Loads the latest selected scene.
    /// </summary>
    public static void StartGameplayScene() {

        SceneManager.LoadSceneAsync(GetScene());

    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public static void RestartGame() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    }

    /// <summary>
    /// Back to the main menu.
    /// </summary>
    public static void MainMenu() {

        SceneManager.LoadSceneAsync(0);

    }

    /// <summary>
    /// Sets the volume of the audiolistener.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetAudioVolume(float volume) {

        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(SGS_Settings.Instance.playerPrefsAudioVolume, volume);

    }

    /// <summary>
    /// Sets the music volume.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetMusicVolume(float volume) {

        PlayerPrefs.SetFloat(SGS_Settings.Instance.playerPrefsMusicVolume, volume);

    }

    /// <summary>
    /// Get volume of the audiolistener.
    /// </summary>
    /// <returns></returns>
    public static float GetAudioVolume() {

        return PlayerPrefs.GetFloat(SGS_Settings.Instance.playerPrefsAudioVolume, SGS_Settings.Instance.defaultAudioVolume);

    }

    /// <summary>
    /// Resets the game by deleting the save data and reloading the scene again.
    /// </summary>
    public static void ResetGame() {

        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    }

}

#else

/// <summary>
/// General API class.
/// </summary>
public class SGS {
}

#endif