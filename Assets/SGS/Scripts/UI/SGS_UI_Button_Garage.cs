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
/// Main menu scene loader button.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Garage Button")]
public class SGS_UI_Button_Garage : MonoBehaviour {

    public int targetSceneIndex = 0;

    private AsyncOperation loadingLevel;

    public void OnClick() {

        if (loadingLevel == null)
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetSceneIndex);

    }

}
