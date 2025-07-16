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

#if BCG_RCCP

/// <summary>
/// Spawns the latest selected vehicle at the spawn point.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/SGS Spawner")]
public class SGS_Spawner : MonoBehaviour {

    /// <summary>
    /// Spawn type.
    /// </summary>
    public enum SpawnTime { Start, Delayed, Script }
    public SpawnTime spawnTime = SpawnTime.Start;

    /// <summary>
    /// Enables the headlights of the vehicle if it's set to true.
    /// </summary>
    public bool isNight = false;

    /// <summary>
    /// Delay time in seconds for delayed spawning.
    /// </summary>
    [Min(0f)] public float delayedTime = 0f;

    /// <summary>
    /// Prevents to have multiple player vehicles in the scene.
    /// </summary>
    public bool preventSpawnIfPlayerExists = true;

    /// <summary>
    /// Auto loads the customization loadout when the vehicle spawned.
    /// </summary>
    public bool autoLoadCustomization = true;

    /// <summary>
    /// Spawned RCCP vehicle.
    /// </summary>
    public RCCP_CarController spawnedPlayer;

    private void Start() {

        // Spawn the vehicle on start.
        if (spawnTime == SpawnTime.Start)
            Spawn();

        //  Spawn the vehicle with delay.
        if (spawnTime == SpawnTime.Delayed)
            StartCoroutine(SpawnDelayed());

    }

    /// <summary>
    /// Spawn the latest selected vehicle with delayer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnDelayed() {

        yield return new WaitForSeconds(delayedTime);
        Spawn();

    }

    /// <summary>
    /// Spawn the latest selected vehicle at the spawn point.
    /// </summary>
    public void Spawn() {

        //  Return if spawned player is not null and prevent option is enabled.
        if (spawnedPlayer != null && preventSpawnIfPlayerExists)
            return;

        //  Getting the spawnable vehicle.
        RCCP_CarController spawnThisVehicle = SGS_PlayerVehicles.Instance.playerVehicles[SGS.GetVehicle()].vehicle;

        //  If spawnable vehicle is null, throw error and return.
        if (!spawnThisVehicle) {

            Debug.LogError("Spawnable vehicle is null, check the player vehicles list!");
            return;

        }

        //  Spawning the player vehicle.
        spawnedPlayer = RCCP.SpawnRCC(spawnThisVehicle, transform.position, transform.rotation, true, true, true);

        if (autoLoadCustomization) {

            //  Loading the customization.
            RCCP_Customizer customizer = spawnedPlayer.Customizer;

            if (customizer) {

                customizer.Load();
                customizer.Initialize();

            } else {

                Debug.Log("RCCP_Customizer couldn't found in the spawned vehicle.");

            }

        }

        RCCP_Lights lights = spawnedPlayer.Lights;

        if (lights) {

            lights.lowBeamHeadlights = isNight;

        } else {

            Debug.Log("RCCP_Lights couldn't found in the spawned vehicle.");

        }

    }

}

#else

/// <summary>
/// Spawns the latest selected vehicle at the spawn point.
/// </summary>
public class SGS_Spawner : MonoBehaviour {
}

#endif