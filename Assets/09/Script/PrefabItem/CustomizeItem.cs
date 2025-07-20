using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CustomizeItem : MonoBehaviour
{
    [SerializeField] private Button itemBtn;
    [SerializeField] private Image itemBgImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image itemLockImage;
    public void SetUp(int index, int type, Button purchaseByCoin, Button purchaseByAds)
    { 
        if (PlayerData.GetStatusCurrentItemCustomize(index, 0) == 1)
        {
            purchaseByCoin.gameObject.SetActive(false);
            purchaseByAds.gameObject.SetActive(false);
            itemLockImage.gameObject.SetActive(false);
        }
        if (type == 0)
        {
            itemBgImage.color = ManageCustomizeSo.Instance.colorDataSo.colors[index].color;
            itemBtn.onClick.AddListener(() => ClickColorItem(index, purchaseByCoin, purchaseByAds));
        }
        else if (type == 1)
        {
            itemImage.sprite = ManageCustomizeSo.Instance.wheelDataSo.wheels[index].wheelImage;
            itemBgImage.sprite = null;
            itemBtn.onClick.AddListener(() => ClickWheelItem(index, purchaseByCoin, purchaseByAds));
        }
    }

    private void ClickColorItem(int index,  Button purchaseByCoin, Button purchaseByAds, int type = 0)
    {
        RCC_CustomizationManager handler = RCC_CustomizationManager.Instance;
        
        if (!handler) {
            Debug.LogError("You are trying to customize the vehicle, but there is no ''RCC_CustomizationManager'' in your scene yet.");
            return;
        }
        
        handler.Paint(ManageCustomizeSo.Instance.colorDataSo.colors[index].color);

        if (PlayerData.GetStatusCurrentItemCustomize(index, 0) == 1)
        {
            purchaseByCoin.gameObject.SetActive(false);
            purchaseByAds.gameObject.SetActive(false);
        }
        else
        {
            if (ManageCustomizeSo.Instance.colorDataSo.colors[index].type == VehicleCollectType.Coins)
            {
                purchaseByCoin.gameObject.SetActive(true);
                purchaseByCoin.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(ManageCustomizeSo.Instance.colorDataSo.colors[index].price.ToString());
                purchaseByAds.gameObject.SetActive(false);
                
                purchaseByCoin.onClick.RemoveAllListeners();
                purchaseByCoin.onClick.AddListener(() => OnClickPurchaseByCoin(index, type, purchaseByCoin));
            }
            else if(ManageCustomizeSo.Instance.colorDataSo.colors[index].type == VehicleCollectType.Ads)
            {
                purchaseByCoin.gameObject.SetActive(false);
                purchaseByAds.gameObject.SetActive(true);
                
                purchaseByAds.onClick.RemoveAllListeners();
                purchaseByAds.onClick.AddListener(() => OnClickPurchaseByAds(index, type, purchaseByAds));
            }
        }
    }

    private void ClickWheelItem(int index, Button purchaseByCoin, Button purchaseByAds, int type = 1)
    {
        RCC_CustomizationManager handler = RCC_CustomizationManager.Instance;

        if (!handler)
        {
            Debug.LogError(
                "You are trying to customize the vehicle, but there is no ''RCC_CustomizationManager'' in your scene yet.");
            return;
        }
        
        handler.ChangeWheels(index);
        
        if (PlayerData.GetStatusCurrentItemCustomize(index, 1) == 1)
        {
            purchaseByCoin.gameObject.SetActive(false);
            purchaseByAds.gameObject.SetActive(false);
        }
        else
        {
            if (ManageCustomizeSo.Instance.wheelDataSo.wheels[index].type == VehicleCollectType.Coins)
            {
                purchaseByCoin.gameObject.SetActive(true);
                purchaseByCoin.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(ManageCustomizeSo.Instance.wheelDataSo.wheels[index].price.ToString());
                purchaseByAds.gameObject.SetActive(false);
                
                purchaseByCoin.onClick.RemoveAllListeners();
                purchaseByCoin.onClick.AddListener(() => OnClickPurchaseByCoin(index, type, purchaseByCoin));
            }
            else if(ManageCustomizeSo.Instance.wheelDataSo.wheels[index].type == VehicleCollectType.Ads)
            {
                purchaseByCoin.gameObject.SetActive(false);
                purchaseByAds.gameObject.SetActive(true);
                
                purchaseByAds.onClick.RemoveAllListeners();
                purchaseByAds.onClick.AddListener(() => OnClickPurchaseByAds(index, type, purchaseByAds));
            }
        }
    }
    
    private void OnClickPurchaseByCoin(int index, int type, Button purchaseBtn)
    {
        var currentCoins = PlayerData.GetMoney();
        var price = 0;
        if(type == 0) 
             price = ManageCustomizeSo.Instance.colorDataSo.colors[index].price;
        else if(type == 1)
             price = ManageCustomizeSo.Instance.wheelDataSo.wheels[index].price;
        
        if (currentCoins >= price)
        {
            itemLockImage.gameObject.SetActive(false);
            purchaseBtn.gameObject.SetActive(false);
            PlayerData.SetMoney(-price);
            PlayerData.SetStatusCurrentItemCustomize(index, type);
        }
    }

    private void OnClickPurchaseByAds(int index, int type, Button purchaseBtn)
    {
        
    }
}
