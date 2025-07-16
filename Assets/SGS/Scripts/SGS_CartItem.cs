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
/// Cart item used in the main menu.
/// </summary>
[System.Serializable]
[AddComponentMenu("BoneCracker Games/SGS/SGS Cart Item")]
public class SGS_CartItem {

    public enum CartItemType { Paint, Wheel, Spoiler, Decal, Neon, UpgradeEngine, UpgradeHandling, UpgradeSpeed }
    public CartItemType itemType;

    public string saveKey;
    public int price;

}
