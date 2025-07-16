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
/// UI button for purchasing the items.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Purchase Item")]
public class SGS_UI_PurchaseItem : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Item.
    /// </summary>
    public SGS_CartItem item;

    /// <summary>
    /// Price panel.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// Price text.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Button to purchase / select.
    /// </summary>
    private Button button;

    /// <summary>
    /// Is purchased?
    /// </summary>
    public bool isPurchased = false;

    private void Awake() {

        //  Getting button.
        button = GetComponent<Button>();

        //  Is purchased?
        isPurchased = CheckPurchase();

        //  Enabling / disabling the price panel depending on the purchased state.
        pricePanel.SetActive(!isPurchased);

        //  Setting price text.
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");

    }

    public void OnEnable() {

        //  Is purchased?
        isPurchased = CheckPurchase();

        //  Enabling / disabling the price panel depending on the purchased state.
        pricePanel.SetActive(!isPurchased);

        //  Setting price text.
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");

    }

    /// <summary>
    /// Is purchased?
    /// </summary>
    /// <returns></returns>
    public bool CheckPurchase() {

        //  Is purchased?
        isPurchased = PlayerPrefs.HasKey(item.saveKey);

        //  Enabling / disabling the price panel depending on the purchased state.
        pricePanel.SetActive(!isPurchased);

        //  Setting price text.
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");

        return isPurchased;

    }

    /// <summary>
    /// On click.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData) {

        //  Return if button is not interactable or disabled.
        if (!button.interactable || !button.gameObject.activeSelf)
            return;

        //  Is purchased?
        isPurchased = CheckPurchase();

        SGS_SceneManager.Instance.CheckItemPurchased(item);

    }

}

#else

/// <summary>
/// UI button for purchasing the items.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Purchase Item")]
public class SGS_UI_PurchaseItem : MonoBehaviour {
}

#endif