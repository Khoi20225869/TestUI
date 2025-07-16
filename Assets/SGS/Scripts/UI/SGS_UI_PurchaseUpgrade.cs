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
/// UI upgrader button for engine, handling, and speed.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Purchase Upgrade")]
public class SGS_UI_PurchaseUpgrade : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Upgradable item.
    /// </summary>
    public SGS_CartItem item;

    /// <summary>
    /// Upgrade level.
    /// </summary>
    public int UpgradeLevel {

        get {

            return int.Parse(button.levelText.text);

        }

    }

    /// <summary>
    /// Calculated upgrade price.
    /// </summary>
    public int UpgradePrice {

        get {

            return (int)(Mathf.InverseLerp(0, 6, UpgradeLevel + 1) * (defaultPrice * 1.5f));

        }

    }

    /// <summary>
    /// Default price.
    /// </summary>
    private int defaultPrice = 0;

    /// <summary>
    /// Maximum level.
    /// </summary>
    public int maximumLevel = 5;

    /// <summary>
    /// Price panel.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// Price text.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// RCCP Upgrader button.
    /// </summary>
    private RCCP_UI_Upgrade button;

    /// <summary>
    /// Is upgraded completely?
    /// </summary>
    public bool isUpgraded = false;

    private void Awake() {

        //  Getting the upgrader button.
        button = GetComponent<RCCP_UI_Upgrade>();

        //  Checking the upgrade state.
        isUpgraded = CheckPurchase();

        //  Getting the default price.
        defaultPrice = item.price;

        //  Enabling / disabling the price panel depending on the upgrade state.
        pricePanel.SetActive(!isUpgraded);

        //  Setting price text.
        priceText.text = isUpgraded ? "" : "$" + item.price.ToString("F0");

    }

    public void OnEnable() {

        StartCoroutine(OnEnableDelayed());

    }

    private IEnumerator OnEnableDelayed() {

        //  Getting the upgrader button.
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        //  Return if upgrader button not found yet.
        if (!button)
            yield break;

        button.enabled = false;
        yield return new WaitForEndOfFrame();
        button.enabled = true;

        //  Is upgraded?
        isUpgraded = CheckPurchase();

        //  Enabling / disabling the price panel depending on the upgrade state.
        pricePanel.SetActive(!isUpgraded);

        //  Setting price text.
        priceText.text = isUpgraded ? "" : "$" + UpgradePrice.ToString("F0");

    }

    /// <summary>
    /// Checking if the item is upgraded completely or not.
    /// </summary>
    /// <returns></returns>
    public bool CheckPurchase() {

        //  Getting the upgrader button.
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        //  Return if upgrader button not found yet.
        if (!button)
            return false;

        //  If level is at maximum level, return true. Otherwise false.
        if (UpgradeLevel >= maximumLevel)
            isUpgraded = true;
        else
            isUpgraded = false;

        //  Enabling / disabling the price panel depending on the upgrade state.
        pricePanel.SetActive(!isUpgraded);

        //  Setting price text.
        priceText.text = isUpgraded ? "" : "$" + UpgradePrice.ToString("F0");

        return isUpgraded;

    }

    /// <summary>
    /// On click.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) {

        //  Getting the upgrader button.
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        //  Return if upgrader button not found yet.
        if (!button)
            return;

        //  Return if upgrader button is not interactable or active.
        if (!button.isActiveAndEnabled || !button.gameObject.activeSelf)
            return;

        //  Is upgraded?
        isUpgraded = CheckPurchase();

        SGS_CartItem upgradeItem = item;
        upgradeItem.price = UpgradePrice;
        SGS_SceneManager.Instance.CheckUpgradePurchased(item);

    }

    /// <summary>
    /// Refresh the upgrader button.
    /// </summary>
    public void Refresh() {

        //  Getting the upgrader button.
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        //  Return if upgrader button not found yet.
        if (!button)
            return;

        ///// Refresh the upgrader button.
        //if (button)
        //    button.Refresh();

    }

}

#else

/// <summary>
/// UI upgrader button for engine, handling, and speed.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Purchase Upgrade")]
public class SGS_UI_PurchaseUpgrade : MonoBehaviour {
}

#endif