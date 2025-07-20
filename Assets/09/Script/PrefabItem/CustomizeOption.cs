using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CustomizeOption : MonoBehaviour
{
    [SerializeField] private Button optionBtn;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject itemPrefab;
    public void Setup(int index, Transform itemContent, Button purchaseByCoin, Button purchaseByAds)
    {
        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.AddListener(() => OnButtonClick(index, itemContent, purchaseByCoin, purchaseByAds));

        if (index == 0)
            StartCoroutine(SpawnItem(0, itemContent, purchaseByCoin, purchaseByAds));
    }

    private void OnButtonClick(int index, Transform itemContent, Button purchaseByCoin, Button purchaseByAds)
    {
        if (CustomizePage.CurrentOptionIndex == index) return;
        foreach(Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
        CustomizePage.CurrentOptionIndex = index;
        StartCoroutine(SpawnItem(index, itemContent, purchaseByCoin, purchaseByAds));
    }

    private IEnumerator SpawnItem(int index, Transform itemContent,  Button purchaseByCoin, Button purchaseByAds)
    {
        if (index == 0)
        {
            for( int i = 0 ;  i<  ManageCustomizeSo.Instance.colorDataSo.colors.Length; i ++)
            {
                var itemColor = Instantiate(itemPrefab, itemContent);
                var colorItem = itemColor.GetComponent<CustomizeItem>();
                colorItem.SetUp(i, index, purchaseByCoin, purchaseByAds);
                yield return new WaitForSeconds(0.02f);
            }
        }
        else if (index == 1)
        {
            for(int i = 0 ; i < ManageCustomizeSo.Instance.wheelDataSo.wheels.Length ; i++)
            {
                var itemWheel = Instantiate(itemPrefab, itemContent);
                var wheelItem = itemWheel.GetComponent<CustomizeItem>();
                wheelItem.SetUp(i, index, purchaseByCoin, purchaseByAds);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
