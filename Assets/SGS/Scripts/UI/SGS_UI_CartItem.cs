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
using TMPro;

/// <summary>
/// UI cart item displayed in the cart panel.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Cart Item")]
public class SGS_UI_CartItem : MonoBehaviour {

    /// <summary>
    /// Item type and properties.
    /// </summary>
    public SGS_CartItem item;

    /// <summary>
    /// Item name text.
    /// </summary>
    public TextMeshProUGUI itemNameText;

    /// <summary>
    /// Item price text.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Sets the new item.
    /// </summary>
    /// <param name="newItem"></param>
    public void SetItem(SGS_CartItem newItem) {

        item = newItem;

        itemNameText.text = item.itemType.ToString();
        priceText.text = "$ " + item.price.ToString("F0");

    }

}
