using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CustomizeOption_09 : MonoBehaviour
{
    [SerializeField] private Button optionBtn;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject itemPrefab;
    public void Setup(int index, Transform itemContent)
    {
        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.AddListener(() => OnButtonClick(index, itemContent));
        
        if(index == 0)
            StartCoroutine(SpawnItem(0, itemContent));
    }

    private void OnButtonClick(int index, Transform itemContent)
    {
        if (CustomizePage_09.CurrentOptionIndex == index) return;
        foreach(Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }
        CustomizePage_09.CurrentOptionIndex = index;
        StartCoroutine(SpawnItem(index, itemContent));
    }

    private IEnumerator SpawnItem(int index, Transform itemContent)
    {
        if (index == 0)
        {
            for( int i = 0 ;  i<  ManageCustomizeSo.Instance.colorDataSo.colors.Length; i ++)
            {
                var itemColor = Instantiate(itemPrefab, itemContent);
                var colorItem = itemColor.GetComponent<CustomizeItem_09>();
                colorItem.SetUp(i, index);
                yield return new WaitForSeconds(0.02f);
            }
        }
        else if (index == 1)
        {
            for(int i = 0 ; i < ManageCustomizeSo.Instance.wheelDataSo.wheels.Length ; i++)
            {
                var itemWheel = Instantiate(itemPrefab, itemContent);
                var wheelItem = itemWheel.GetComponent<CustomizeItem_09>();
                wheelItem.SetUp(i, index);
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
