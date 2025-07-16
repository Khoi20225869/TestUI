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
using UnityEditor.Callbacks;

public class SGS_InitLoad {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    public static void EditorUpdate() {

        bool hasKey = false;

#if BCG_SGS
        hasKey = true;
#endif

        if (!hasKey) {

            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using Simple Garage System for Realistic Car Controller Pro. Please read the documentation before use. Have fun :)", "Let's get started!");
            EditorUtility.DisplayDialog("Searching Realistic Car Controller Pro", "Simple Garage System will search for Realistic Car Controller Pro in the project. Simple Garage System won't work without it.", "Ok");

            SGS_SetScriptingSymbol.SetEnabled("BCG_SGS", true);

        }

    }

    [DidReloadScripts]
    public static void CheckRCCP() {

        EditorApplication.delayCall += () => {

            bool hasRCCPKey = false;

#if BCG_RCCP
            hasRCCPKey = true;
#endif

            if (!hasRCCPKey) {

                if (EditorUtility.DisplayDialog("Realistic Car Controller Pro needed", "Simple Garage System requires Realistic Car Controller Pro imported and installed properly. Please import and install the latest version of Realistic Car Controller Pro. If you don't own the Realistic Car Controller Pro, you can remove the Simple Garage System from the project safely.", "Import Realistic Car Controller Pro", "Dismiss"))
                    Application.OpenURL("https://u3d.as/22Bf");

            }

        };

    }

}
