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
using UnityEditor;

#if BCG_RCCP

[CustomEditor(typeof(SGS_PlayerVehicles))]
public class SGS_PlayerVehiclesEditor : Editor {

    SGS_PlayerVehicles prop;
    static bool autoUpdate = true;

    public override void OnInspectorGUI() {

        prop = (SGS_PlayerVehicles)target;
        serializedObject.Update();

        autoUpdate = EditorGUILayout.ToggleLeft("Auto Update Prefabs", autoUpdate);
        EditorGUILayout.Separator();

        if (!autoUpdate && GUILayout.Button("Update"))
            UpdateRCCPVehicles();

        EditorGUILayout.HelpBox("Adjusting values would overwrite the vehicle settings.", MessageType.Info);

        EditorGUI.indentLevel++;
        DrawDefaultInspector();
        EditorGUI.indentLevel--;

        for (int i = 0; i < prop.playerVehicles.Length; i++) {

            if (prop.playerVehicles[i] != null && prop.playerVehicles[i].vehicle != null) {

                if (!prop.playerVehicles[i].vehicle.gameObject.TryGetComponent(out SGS_Player playerComponent)) {

                    prop.playerVehicles[i].vehicle.gameObject.AddComponent<SGS_Player>();
                    EditorUtility.SetDirty(prop);

                }

            }

        }

        if (GUILayout.Button("Create New Vehicle Slot")) {

            SGS_PlayerVehicles.PlayerVehicle newVehicle = new SGS_PlayerVehicles.PlayerVehicle();
            prop.AddNewVehicle(newVehicle);

        }

        if (GUI.changed) {

            EditorUtility.SetDirty(prop);

            if (autoUpdate)
                UpdateRCCPVehicles();

        }

        serializedObject.ApplyModifiedProperties();

    }

    /// <summary>
    /// Updates all RCCP vehicles with given settings in the CCDS_PlayerVehicles (Tools --> BCG --> CCDS --> Player Vehicles).
    /// </summary>
    private void UpdateRCCPVehicles() {

        for (int i = 0; i < SGS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            SGS_PlayerVehicles.PlayerVehicle vehicle = SGS_PlayerVehicles.Instance.playerVehicles[i];

            if (vehicle.vehicle != null) {

                RCCP_Engine engine = vehicle.vehicle.GetComponentInChildren<RCCP_Engine>(true);
                RCCP_Stability stability = vehicle.vehicle.GetComponentInChildren<RCCP_Stability>(true);
                RCCP_Differential differential = vehicle.vehicle.GetComponentInChildren<RCCP_Differential>(true);

                if (!vehicle.initialized) {

                    if (engine)
                        vehicle.engineTorque = engine.maximumTorqueAsNM;

                    if (stability)
                        vehicle.handling = (stability.tractionHelperStrength) * 1f;

                    if (differential)
                        vehicle.speed = Mathf.Lerp(160f, 380f, Mathf.InverseLerp(5.31f, 2.8f, differential.finalDriveRatio));

                    vehicle.initialized = true;

                }

                if (engine)
                    engine.maximumTorqueAsNM = vehicle.engineTorque;

                if (stability) {

                    stability.tractionHelperStrength = vehicle.handling * 1f;

                }

                if (differential) {

                    differential.finalDriveRatio = Mathf.Lerp(5.31f, 2.8f, Mathf.InverseLerp(160f, 380f, vehicle.speed));

                }

                RCCP_VehicleUpgrade_Engine engineUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Engine>(true);
                RCCP_VehicleUpgrade_Handling stabilityUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Handling>(true);
                RCCP_VehicleUpgrade_Speed speedUpgrade = vehicle.vehicle.GetComponentInChildren<RCCP_VehicleUpgrade_Speed>(true);

                if (engineUpgrade)
                    engineUpgrade.efficiency = vehicle.upgradedEngineEfficiency;

                if (stabilityUpgrade)
                    stabilityUpgrade.efficiency = vehicle.upgradedHandlingEfficiency;

                if (speedUpgrade)
                    speedUpgrade.efficiency = vehicle.upgradedSpeedEfficiency;

                EditorUtility.SetDirty(vehicle.vehicle.gameObject);
                PrefabUtility.SavePrefabAsset(vehicle.vehicle.gameObject);

            } else {

                vehicle.initialized = false;

            }

        }

        serializedObject.ApplyModifiedProperties();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

}

#else

[CustomEditor(typeof(SGS_PlayerVehicles))]
public class SGS_PlayerVehiclesEditor : Editor {
}

#endif