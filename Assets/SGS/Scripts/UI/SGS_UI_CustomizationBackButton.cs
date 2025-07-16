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
/// Back UI button used in the main menu customization panel. Used to prevent lefted items in the cart.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Customization Back Button")]
public class SGS_UI_CustomizationBackButton : MonoBehaviour {

    /// <summary>
    /// Main menu panel.
    /// </summary>
    public GameObject mainMenuPanel;

    /// <summary>
    /// On click.
    /// </summary>
    public void OnClick() {

        //  Inform if player left items in the cart before going back.
        if (SGS_SceneManager.Instance.itemsInCart.Count > 0) {

            SGS_UI_Informer.Instance.Info("You've left items in your cart, please clear or purchase the items!");
            return;

        }

        //  Get back to the main menu.
        SGS_SceneManager.Instance.OpenPanel(mainMenuPanel);

        //  And set panel title text.
        SGS_SceneManager.Instance.SetPanelTitleText("Maýn Menu");

    }

}

#else

/// <summary>
/// Back UI button used in the main menu customization panel. Used to prevent lefted items in the cart.
/// </summary>
[AddComponentMenu("BoneCracker Games/CCDS/UI/CCDS UI Customization Back Button")]
public class SGS_UI_CustomizationBackButton : MonoBehaviour {
}

#endif