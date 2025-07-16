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
using System.Linq;
using UnityEngine;

#if BCG_RCCP

/// <summary>
/// All selectable player vehicles.
/// </summary>
public class SGS_PlayerVehicles : ScriptableObject {

    private static SGS_PlayerVehicles instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static SGS_PlayerVehicles Instance { get { if (instance == null) instance = Resources.Load("SGS_PlayerVehicles") as SGS_PlayerVehicles; return instance; } }

    [System.Serializable]
    public class PlayerVehicle {

        public bool initialized = false;
        public RCCP_CarController vehicle;
        [Min(0)] public int price = 0;

        [Space()] [Range(100f, 1400f)] public float engineTorque = 350f;
        [Range(0f, 1f)] public float handling = .1f;
        [Range(160f, 380f)] public float speed = 240f;

        [Space()] [Range(1f, 2f)] public float upgradedEngineEfficiency = 1.2f;
        [Range(1f, 2f)] public float upgradedHandlingEfficiency = 1.2f;
        [Range(1f, 2f)] public float upgradedSpeedEfficiency = 1.2f;

    }

    /// <summary>
    /// All selectable player vehicles.
    /// </summary>
    public PlayerVehicle[] playerVehicles;

    /// <summary>
    /// Adds a new vehicle to the player vehicles list.
    /// </summary>
    /// <param name="newVehicle"></param>
    public void AddNewVehicle(PlayerVehicle newVehicle) {

        List<PlayerVehicle> currentVehicles = new List<PlayerVehicle>();
        currentVehicles = playerVehicles.ToList();
        currentVehicles.Add(newVehicle);
        playerVehicles = currentVehicles.ToArray();

    }

}

#else

/// <summary>
/// All selectable player vehicles.
/// </summary>
public class SGS_PlayerVehicles : ScriptableObject {
}

#endif