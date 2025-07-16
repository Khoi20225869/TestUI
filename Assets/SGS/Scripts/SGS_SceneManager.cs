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
using UnityEngine.UI;

#if BCG_RCCP

/// <summary>
/// Main scene manager. Main menu scene must have this manager. This manager will be created automatically.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/SGS Scene Manager")]
public class SGS_SceneManager : MonoBehaviour {

    private static SGS_SceneManager instance;

    /// <summary>
    /// Instance of the class.
    /// </summary>
    public static SGS_SceneManager Instance {

        get {

            if (instance == null)
                instance = FindObjectOfType<SGS_SceneManager>();

            return instance;

        }

    }

    /// <summary>
    /// All selectable player vehicles will be pulled from the SGS_PlayerVehicle scriptable object.
    /// </summary>
    private List<RCCP_CarController> allPlayerVehicles = new List<RCCP_CarController>();

    /// <summary>
    /// Spawn point.
    /// </summary>
    [Header("Spawn Point")]
    public Transform spawnPoint;

    /// <summary>
    /// Current vehicle (regardless selected / not selected).
    /// </summary>
    [Header("Current Vehicle")]
    public RCCP_CarController currentVehicle;

    /// <summary>
    /// UI panels.
    /// </summary>
    [Header("UI Panels")]
    public GameObject[] panels;

    /// <summary>
    /// UI texts.
    /// </summary>
    [Header("UI Texts")]
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI panelTitleText;
    public TextMeshProUGUI vehiclePriceText;

    /// <summary>
    /// UI buttons.
    /// </summary>
    [Header("UI Buttons")]
    public GameObject selectVehicleButton;
    public GameObject purchaseVehicleButton;
    public GameObject purchaseCartButton;

    /// <summary>
    /// UI sliders for the vehicle stats.
    /// </summary>
    [Header("UI Sliders For Vehicle Stats")]
    public Image vehicleStats_Engine;
    public Image vehicleStats_Handling;
    public Image vehicleStats_Speed;

    /// <summary>
    /// UI sliders for the upgraded vehicle stats.
    /// </summary>
    [Space()] public Image vehicleStats_Engine_Upgraded;
    public Image vehicleStats_Handling_Upgraded;
    public Image vehicleStats_Speed_Upgraded;

    /// <summary>
    /// Selected vehicle index.
    /// </summary>
    public int selectedVehicleIndex = 0;

    /// <summary>
    /// UI cart panel.
    /// </summary>
    [Header("Cart")]
    public GameObject cartPanel;

    /// <summary>
    /// Target content gameobject. All children UI cart items will be located under this gameobject.
    /// </summary>
    public GameObject cartItemsContent;

    /// <summary>
    /// Prefab or gameobject to be used to instantiate new UI cart items.
    /// </summary>
    public SGS_UI_CartItem cartItemReference;
    private SGS_UI_PurchaseItem[] itemPurchaseButtons;

    /// <summary>
    /// All purchasable items in the cart (not purchased yet).
    /// </summary>
    public List<SGS_CartItem> itemsInCart = new List<SGS_CartItem>();

    private void Awake() {

        //  Creating a clean list for all player vehicles.
        allPlayerVehicles = new List<RCCP_CarController>();

        //  Getting selected vehicle index.
        selectedVehicleIndex = GetVehicleIndex();

    }

    private void Start() {

        //  Spawning all selectable player vehicles once and disabling all of them.
        SpawnAllPlayerVehicles();

        //  After that, only enabling the selected vehicle. Disabling all other vehicles except the selected vehicle.
        EnableVehicle();

    }

    private void Update() {

        //  Displaying the money text.
        cashText.text = "$ " + SGS.GetMoney().ToString("F0");

        //  If current vehicle is not null, display stats of the vehicle.
        if (currentVehicle) {

            //  Fill amount of the engine torque.
            if (vehicleStats_Engine && currentVehicle.Engine)
                vehicleStats_Engine.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Engine.maximumTorqueAsNM);

            //  Fill amount of the stability strength.
            if (vehicleStats_Handling && currentVehicle.Stability)
                vehicleStats_Handling.fillAmount = Mathf.InverseLerp(0f, .5f, (currentVehicle.Stability.tractionHelperStrength) * 1f);

            //  Fill amount of the speed.
            if (vehicleStats_Speed && currentVehicle.Differential)
                vehicleStats_Speed.fillAmount = 1f - Mathf.InverseLerp(2.8f, 5.31f, currentVehicle.Differential.finalDriveRatio);

            //  Fill amount of the upgraded engine torque.
            if (vehicleStats_Engine_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Engine)
                vehicleStats_Engine_Upgraded.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Customizer.UpgradeManager.Engine.defEngine * currentVehicle.Customizer.UpgradeManager.Engine.efficiency);
            else if (vehicleStats_Engine_Upgraded)
                vehicleStats_Engine_Upgraded.fillAmount = 0f;

            //  Fill amount of the upgraded handling strength.
            if (vehicleStats_Handling_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Handling)
                vehicleStats_Handling_Upgraded.fillAmount = Mathf.InverseLerp(0f, .5f, currentVehicle.Customizer.UpgradeManager.Handling.defHandling * currentVehicle.Customizer.UpgradeManager.Handling.efficiency);
            else if (vehicleStats_Handling_Upgraded)
                vehicleStats_Handling_Upgraded.fillAmount = 0f;

            //Fill amount of the upgraded speed.
            if (vehicleStats_Speed_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Speed)
                vehicleStats_Speed_Upgraded.fillAmount = 1f - Mathf.InverseLerp(2.8f, 5.31f, Mathf.Lerp(currentVehicle.Customizer.UpgradeManager.Speed.defRatio, currentVehicle.Customizer.UpgradeManager.Speed.defRatio * .6f, currentVehicle.Customizer.UpgradeManager.Speed.efficiency - 1f));
            else if (vehicleStats_Speed_Upgraded)
                vehicleStats_Speed_Upgraded.fillAmount = 0f;

            //  Enabling or disabling select and purchase buttons.
            selectVehicleButton.SetActive(SGS.IsOwnedVehicle(selectedVehicleIndex));
            purchaseVehicleButton.SetActive(!selectVehicleButton.activeSelf);

            //  If purchase button is active, set text of the price.
            if (purchaseVehicleButton.activeSelf)
                vehiclePriceText.text = "$ " + SGS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price.ToString();
            else
                vehiclePriceText.text = "";

            if (cartPanel.activeInHierarchy)
                UpdateCartItemsList();

        }

    }

    /// <summary>
    /// Spawns all selectable player vehicles once.
    /// </summary>
    private void SpawnAllPlayerVehicles() {

        //  If spawn point couldn't found, inform and create it.
        if (spawnPoint == null) {

            Debug.LogError("Spawn point couldn't found, creating it at vector3 zero. Be sure to create a spawn point and assign it in the CCDS_MainMenuManager!");

            spawnPoint = new GameObject("CCDS_SpawnPoint").transform;
            spawnPoint.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnPoint.transform.position += Vector3.up * 1.5f;
            spawnPoint.transform.position += Vector3.forward * 15f;

        }

        //  Spawning all selectable player vehicles, disabling them, and enabling or disabling the headlights as well.
        for (int i = 0; i < SGS_PlayerVehicles.Instance.playerVehicles.Length; i++) {

            RCCP_CarController spawned = RCCP.SpawnRCC(SGS_PlayerVehicles.Instance.playerVehicles[i].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation, true, false, false);
            allPlayerVehicles.Add(spawned);

            if (spawned.Lights)
                spawned.Lights.lowBeamHeadlights = true;
            else
                Debug.LogWarning("Lights couldn't found on this player vehicle named " + spawned.transform.name + ", please add lights component through the RCCP_CarController!");

            if (spawned.Customizer) {

                if (!PlayerPrefs.HasKey(spawned.Customizer.saveFileName)) {

                    spawned.Customizer.Save();

                } else {

                    spawned.Customizer.Load();
                    spawned.Customizer.Initialize();

                }

            } else {

                Debug.LogWarning("Customizer couldn't found on this player vehicle named " + spawned.transform.name + ", please add customizer component through the RCCP_CarController!");

            }

            spawned.gameObject.SetActive(false);

        }

    }

    /// <summary>
    /// Gets the latest selected vehicle as int index.
    /// </summary>
    /// <returns></returns>
    public int GetVehicleIndex() {

        return SGS.GetVehicle();

    }

    /// <summary>
    /// Saves the current vehicle as selected player vehicle.
    /// </summary>
    public void SelectVehicle() {

        SGS.SetVehicle(selectedVehicleIndex);

    }

    /// <summary>
    /// Enables next vehicle and disables all other ones.
    /// </summary>
    public void NextVehicle() {

        selectedVehicleIndex++;

        if (selectedVehicleIndex >= SGS_PlayerVehicles.Instance.playerVehicles.Length)
            selectedVehicleIndex = 0;

        EnableVehicle();

    }

    /// <summary>
    /// Enables previous vehicle and disables all other ones.
    /// </summary>
    public void PreviousVehicle() {

        selectedVehicleIndex--;

        if (selectedVehicleIndex < 0)
            selectedVehicleIndex = SGS_PlayerVehicles.Instance.playerVehicles.Length - 1;

        EnableVehicle();

    }

    /// <summary>
    /// Enables the current vehicle and disables all other ones.
    /// </summary>
    public void EnableVehicle() {

        //  Disabling all vehicles.
        for (int i = 0; i < allPlayerVehicles.Count; i++) {

            if (allPlayerVehicles[i] != null)
                allPlayerVehicles[i].gameObject.SetActive(false);

        }

        //  If selected vehicle is not null, save it as a player vehicle and register it.
        if (allPlayerVehicles[selectedVehicleIndex] != null) {

            currentVehicle = allPlayerVehicles[selectedVehicleIndex];
            allPlayerVehicles[selectedVehicleIndex].gameObject.SetActive(true);
            RCCP.RegisterPlayerVehicle(allPlayerVehicles[selectedVehicleIndex], false, false);

        }

        //  If price of the vehicle is below 0, purchase it automatically.
        if (SGS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price <= 0)
            SGS.UnlockVehicle(selectedVehicleIndex);

    }

    /// <summary>
    /// Purchases the current vehicle.
    /// </summary>
    public void PurchaseVehicle() {

        //  Getting current money.
        int currentMoney = SGS.GetMoney();

        //  If current money is enough to purchase the vehicle, purchase it.
        if (currentMoney >= SGS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price) {

            //  Purchasing the vehicle and changing the amount of the money.
            SGS.UnlockVehicle(selectedVehicleIndex);
            SGS.ChangeMoney(-SGS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price);

            //  Displaying text.
            SGS_UI_Informer.Instance.Info("New vehicle has been purchased!");

            //  And reenabling the vehicle.
            EnableVehicle();

        } else {

            //  If current money is not enough to purchase the vehicle, display text.
            int difference = SGS_PlayerVehicles.Instance.playerVehicles[selectedVehicleIndex].price - currentMoney;
            SGS_UI_Informer.Instance.Info(difference + " more money needed to purchase this vehicle!");

        }

    }

    /// <summary>
    /// Selects the target scene.
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SelectScene(int sceneIndex) {

        SGS.SetScene(sceneIndex);

    }

    /// <summary>
    /// Starts the target scene.
    /// </summary>
    public void StartScene() {

        SaveCustomization();

        SGS.StartGameplayScene();

    }

    public void OpenPanel(GameObject activePanel) {

        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(false);

        if (activePanel)
            activePanel.SetActive(true);

    }

    /// <summary>
    /// Checks upgradable item purchased or not. If not purchased, add to the cart, remove otherwise.
    /// </summary>
    /// <param name="newItem"></param>
    public void CheckUpgradePurchased(SGS_CartItem newItem) {

        if (!currentVehicle.CarController.Customizer) {

            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
            return;

        }

        if (PlayerPrefs.HasKey(currentVehicle.CarController.Customizer.saveFileName + newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

    }

    /// <summary>
    /// Checks purchasable item. If not purchased, add to the cart, remove otherwise.
    /// </summary>
    /// <param name="newItem"></param>
    public void CheckItemPurchased(SGS_CartItem newItem) {

        if (PlayerPrefs.HasKey(newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

    }

    /// <summary>
    /// Adds a new item to the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddItemToCart(SGS_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (!itemsInCart.Contains(newItem))
            itemsInCart.Add(newItem);

    }

    /// <summary>
    /// Removes an item from the cart. Cart can't have items with same type.
    /// </summary>
    /// <param name="newItem"></param>
    public void RemoveItemFromCart(SGS_CartItem newItem) {

        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null) {

                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);

            }

        }

        if (itemsInCart.Contains(newItem))
            itemsInCart.Remove(newItem);

    }

    /// <summary>
    /// Clears the cart and restores the player vehicle back to the last loadout.
    /// </summary>
    public void ClearCart() {

        itemsInCart.Clear();

        LoadCustomization();
        ApplyCustomization();

        if (currentVehicle.Customizer.WheelManager && currentVehicle.Customizer.WheelManager.wheelIndex == -1)
            currentVehicle.Customizer.WheelManager.Restore();

        if (currentVehicle.Customizer.PaintManager && currentVehicle.Customizer.PaintManager.color == new Color(1f, 1f, 1f, 0f))
            currentVehicle.Customizer.PaintManager.Restore();

        //  Updating all purchasable items in the scene.
        SGS_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<SGS_UI_PurchaseItem>();

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        //  Updating all upgradable items in the scene.
        SGS_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<SGS_UI_PurchaseUpgrade>();

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Purchases all items in the cart and saves the player vehicle loadout..
    /// </summary>
    public void PurchaseCart() {

        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        //  If player money is enough to purchase the cart, proceed. Otherwise return.
        if (SGS.GetMoney() < totalPrice) {

            SGS_UI_Informer.Instance.Info("Not enough money to purchase the cart!");
            return;

        }

        //  Consuming the money.
        SGS.ChangeMoney(-totalPrice);

        //  Saving all purchased items.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                PlayerPrefs.SetInt(itemsInCart[i].saveKey, 1);

        }

        //  Saving the loadout.
        SaveCustomization();

        //  Clearing the cart.
        itemsInCart.Clear();

        //  Updating all purchasable items in the scene.
        SGS_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsOfType<SGS_UI_PurchaseItem>();

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        //  Updating all upgradable items in the scene.
        SGS_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsOfType<SGS_UI_PurchaseUpgrade>();

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    /// <summary>
    /// Updates all items in the cart list.
    /// </summary>
    public void UpdateCartItemsList() {

        //  Getting all items in the cart.
        SGS_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<SGS_UI_CartItem>(true);

        //  Destroying all items before instantiating them.
        foreach (SGS_UI_CartItem item in items) {

            if (!Equals(item.gameObject, cartItemReference.gameObject))
                Destroy(item.gameObject);
            else if (cartItemReference.gameObject.activeSelf)
                cartItemReference.gameObject.SetActive(false);

        }

        //  Instantiating all items in the cart and setting them.
        for (int i = 0; i < itemsInCart.Count; i++) {

            SGS_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<SGS_UI_CartItem>();
            cartItem.gameObject.SetActive(true);
            cartItem.SetItem(itemsInCart[i]);

        }

        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < itemsInCart.Count; i++) {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        //  Enable the purchase button if total price is above 0. Disable otherwise.
        if (totalPrice > 0)
            purchaseCartButton.SetActive(true);
        else
            purchaseCartButton.SetActive(false);

        //  Set price text if purchase button is enabled.
        if (purchaseCartButton.activeSelf)
            purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

    }

    /// <summary>
    /// Saves the current loadout.
    /// </summary>
    public void SaveCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

        SGS_UI_Informer.Instance.Info("Customization saved!");

    }

    /// <summary>
    /// Loads the latest loadout.
    /// </summary>
    public void LoadCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Applies the loaded loadout.
    /// </summary>
    public void ApplyCustomization() {

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Adding money for testing purposes.
    /// </summary>
    public void Testing_AddMoney() {

        SGS.ChangeMoney(10000);

    }

    /// <summary>
    /// Unlocking all vehicles for testing purposes.
    /// </summary>
    public void Testing_UnlockAllCars() {

        SGS.UnlockAllVehicles();

    }

    /// <summary>
    /// Deletes the save data and restarts the game for testing purposes.
    /// </summary>
    public void Testing_ResetSave() {

        SGS.ResetGame();

    }

    /// <summary>
    /// Sets title text.
    /// </summary>
    /// <param name="title"></param>
    public void SetPanelTitleText(string title) {

        panelTitleText.text = title;

    }

}

#else

/// <summary>
/// Main scene manager. Main menu scene must have this manager. This manager will be created automatically.
/// </summary>
public class SGS_SceneManager : MonoBehaviour {
}

#endif