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
/// Player component must be attached to the player vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/SGS Player")]
[RequireComponent(typeof(RCCP_CarController))]
public class SGS_Player : MonoBehaviour {

    /// <summary>
    /// Save name of the vehicle. Must be *unique*.
    /// </summary>
    public string vehicleSaveName = "";

    private RCCP_CarController carController;

    /// <summary>
    /// Car controller.
    /// </summary>
    public RCCP_CarController CarController {

        get {

            if (carController == null)
                carController = GetComponent<RCCP_CarController>();

            return carController;

        }

    }

    private void OnValidate() {

        if (vehicleSaveName == "")
            vehicleSaveName = "Player_" + transform.name;

        RCCP_Customizer customizer = GetComponentInChildren<RCCP_Customizer>();

        if (customizer) {

            customizer.autoSave = false;
            customizer.autoLoadLoadout = false;

        } else {

            Debug.LogWarning("Vehicle named " + vehicleSaveName + " has missing or disabled customizer component (RCCP_Customizer). Customization will not work with this vehicle. Add customizer addon component through the RCCP panel.");

        }

    }

}

#else

/// <summary>
/// Player component must be attached to the player vehicle.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/SGS Player")]
public class SGS_Player : MonoBehaviour {
}

#endif