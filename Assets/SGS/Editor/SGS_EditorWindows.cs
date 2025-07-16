//----------------------------------------------
//          Simple Garage System
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

#if BCG_RCCP

public class SGS_EditorWindows : Editor {

    [MenuItem("Tools/BoneCracker Games/SGS/SGS Settings", false, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/SGS Settings", false, -100)]
    public static void OpenSGSSettings() {

        Selection.activeObject = SGS_Settings.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Player Vehicles", false, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Player Vehicles", false, -100)]
    public static void OpenPlayerVehicles() {

        Selection.activeObject = SGS_PlayerVehicles.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Create/Main Menu System", false, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Create/Main Menu System", false, -100)]
    public static void CreateMainMenuSystem() {

        bool choose = EditorUtility.DisplayDialog("Creating a new main menu system", "This will create a new main menu system and delete the old one if found. Your scene shouldn't contain main camera and audiolistener, system already includes it.", "Create", "Cancel");

        if (choose) {

            Camera camera = FindObjectOfType<Camera>();

            if (camera) {

                if (EditorUtility.DisplayDialog("Camera found", "Camera found in the scene. Main menu system already includes it. Delete the old camera?", "Delete", "Keep"))
                    DestroyImmediate(camera.gameObject);

            }

            AudioListener listener = FindObjectOfType<AudioListener>();

            if (listener) {

                if (EditorUtility.DisplayDialog("Audiolistener found", "Audiolistener found in the scene. Main menu system already includes it. Delete the old Audiolistener?", "Delete", "Keep"))
                    DestroyImmediate(listener.gameObject);

            }

            GameObject resourcesRoot = Instantiate(SGS_Settings.Instance.mainMenuResources, Vector3.zero, Quaternion.identity);

            foreach (Transform item in resourcesRoot.GetComponentsInChildren<Transform>()) {

                if (item.parent == resourcesRoot.transform)
                    item.SetParent(null);

            }

            DestroyImmediate(resourcesRoot.gameObject);
            Debug.Log("New main menu system has been created with Simple Garage System!");



        }

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Create/Main Menu System", true, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Create/Main Menu System", true, -100)]
    public static bool CheckCreateMainMenuSystem() {

        if (Application.isPlaying)
            return false;

        return true;

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Create/Gameplay System", false, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Create/Gameplay System", false, -100)]
    public static void CreateGameplaySystem() {

        bool choose = EditorUtility.DisplayDialog("Creating a new gameplay system", "This will create a new gameplay system and delete the old one if found.", "Create", "Cancel");

        if (choose) {

            RCCP_Camera camera = FindObjectOfType<RCCP_Camera>();

            if (!camera) {

                if (EditorUtility.DisplayDialog("RCCP_Camera couldn't found", "RCCP_Camera couldn't found in the scene. Do you want to create it?", "Create RCCP_Camera", "No, I have my own camera"))
                    RCCP_EditorWindows.CreateRCCCamera();

            }

            EditorUtility.DisplayDialog("RCCP Systems", "If this is a blank new scene, you might want to add other RCCP systems such as RCCP_Canvas. You can create them by Tools --> BCG --> RCCP --> Create.", "Ok");

            GameObject resourcesRoot = Instantiate(SGS_Settings.Instance.gameplayResources, Vector3.zero, Quaternion.identity);

            foreach (Transform item in resourcesRoot.GetComponentsInChildren<Transform>()) {

                if (item.parent == resourcesRoot.transform)
                    item.SetParent(null);

            }

            DestroyImmediate(resourcesRoot.gameObject);
            Debug.Log("New gameplay system has been created with Simple Garage System!");

        }

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Create/Gameplay System", true, -100)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Create/Gameplay System", true, -100)]
    public static bool CheckCreateGameplaySystem() {

        if (Application.isPlaying)
            return false;

        return true;

    }

    [MenuItem("Tools/BoneCracker Games/SGS/Help", false, 1000)]
    [MenuItem("GameObject/BoneCracker Games/SGS/Help", false, 1000)]
    public static void Help() {

        EditorUtility.DisplayDialog("Contact", "Please include your invoice number while sending a contact form. I usually respond within a business day.", "Close");

        string url = "http://www.bonecrackergames.com/contact/";
        Application.OpenURL(url);

    }

}

#else

public class SGS_EditorWindows : Editor {
}

#endif