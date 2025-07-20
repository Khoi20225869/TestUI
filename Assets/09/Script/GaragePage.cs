using System.Collections;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine.UI;

/// <summary>
/// Trang garage: hiển thị danh sách xe và xử lý các nút chức năng.
/// </summary>
public class GaragePage : Page
{
    [Header("Prefabs & Layout")]
    [SerializeField] private GameObject carItemPrefab;
    [SerializeField] private Transform  carContent;

    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button customizeBtn;
    [SerializeField] private Button purchaseByCoinBtn;
    [SerializeField] private Button purchaseByAdsBtn;
    [SerializeField] private Button taskBtn;

    private static CarItem _currentItem;           

    #region Page lifecycle
    public override IEnumerator Initialize()
    {
        BindButtonOnce(playBtn,       OnPlayClicked);
        BindButtonOnce(customizeBtn,  OnCustomizeClicked);
        BindButtonOnce(taskBtn,       OnTaskSelected);
        yield break;
    }

    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnItems());
    }

    public override void DidPopEnter()          
    {
        base.DidPopEnter();
        StartCoroutine(SpawnItems());
    }
    #endregion

    #region Spawn & Select
    private IEnumerator SpawnItems()
    {
        foreach (Transform child in carContent)
            Destroy(child.gameObject);

        _currentItem = null;

        var vehicles = VehicleDataSo.Instance.vehicles;

        for (int i = 0; i < vehicles.Length; i++)
        {
            var go   = Instantiate(carItemPrefab, carContent);
            var item = go.GetComponent<CarItem>();

            item.Initialize(vehicles[i], i, OnItemSelected);
            
            if (i == PlayerData.GetCurrentCarIndex())
                _currentItem = item;

            yield return new WaitForSeconds(0.05f);
        }
        
        _currentItem?.SelectImmediate();
        UpdateActionButtons(PlayerData.GetCurrentCarIndex());
    }

    private void OnItemSelected(int index)
    {
        _currentItem?.Unselect();
        
        _currentItem = carContent.GetChild(index).GetComponent<CarItem>();
        _currentItem.SelectAnimated();
        
        if (PlayerData.GetCarUnlockStatus(index) == 1)
            PlayerData.SetCurrentCarIndex(index);
        
        VehicleManager.Instance.SpawnVehicle(index, false);

        UpdateActionButtons(index);
        
        PlayerData.SetSelectedCarIndex(index);  
    }

    private void UpdateActionButtons(int index)
    {
        bool unlocked = PlayerData.GetCarUnlockStatus(index) == 1;
        playBtn.gameObject.SetActive(unlocked);

        var vehicle = VehicleDataSo.Instance.vehicles[index];
        bool purchasableWithCoin = !unlocked && vehicle.collectType == VehicleCollectType.Coins;
        bool purchasableWithAds  = !unlocked && vehicle.collectType == VehicleCollectType.Ads;

        purchaseByCoinBtn.gameObject.SetActive(purchasableWithCoin);
        purchaseByAdsBtn .gameObject.SetActive(purchasableWithAds );
    }
    #endregion

    #region Navigation buttons
    private void OnPlayClicked()
    {
        CleanupAndPush("SelectModePage9");
        VehicleManager.Instance.DestroyCar();
    }

    private void OnCustomizeClicked() => CleanupAndPush("CustomizePage9");

    private void OnTaskSelected()
    {
        UpdateActionButtons(PlayerData.GetCurrentCarIndex());
        CleanupAndPush("MissionModal9");
    }
    #endregion

    #region Helpers
    private void CleanupAndPush(string pageName)
    {
        StopAllCoroutines();
        foreach (Transform child in carContent)
            Destroy(child.gameObject);

        StartCoroutine(PageContainer.Of(transform).Push(pageName, playAnimation: true));
    }

    private static void BindButtonOnce(Button btn, UnityEngine.Events.UnityAction act)
    {
        if (btn == null) return;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(act);
    }
    #endregion
}
