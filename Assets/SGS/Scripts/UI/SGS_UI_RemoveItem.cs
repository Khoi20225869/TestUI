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
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

#if BCG_RCCP

/// <summary>
/// UI button for removing the item type.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Remove Item")]
public class SGS_UI_RemoveItem : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Item.
    /// </summary>
    public SGS_CartItem item;

    /// <summary>
    /// On click.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) {

        SGS_SceneManager.Instance.RemoveItemFromCart(item);

    }

}

#else

/// <summary>
/// UI button for removing the item type.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Remove Item")]
public class SGS_UI_RemoveItem : MonoBehaviour {
}

#endif