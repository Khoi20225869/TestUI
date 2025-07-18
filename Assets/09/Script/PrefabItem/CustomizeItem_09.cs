using UnityEngine;
using UnityEngine.UI;
public class CustomizeItem_09 : MonoBehaviour
{
    [SerializeField] private Button itemBtn;
    [SerializeField] private Image itemBgImage;
    [SerializeField] private Image itemImage;
    public void SetUp(int index, int type)
    { 
        if (type == 0)
        {
            itemBgImage.color = ManageCustomizeSo.Instance.colorDataSo.colors[index].color;
            itemBtn.onClick.AddListener(() => ClickColorItem(index));
        }
        else if (type == 1)
        {
            itemImage.sprite = ManageCustomizeSo.Instance.wheelDataSo.wheels[index].wheelImage;
            itemBgImage.sprite = null;
            itemBtn.onClick.AddListener(() => ClickWheelItem(index));
        }
    }

    private void ClickColorItem(int index)
    {
        RCC_CustomizationManager handler = RCC_CustomizationManager.Instance;
        
        if (!handler) {
            Debug.LogError("You are trying to customize the vehicle, but there is no ''RCC_CustomizationManager'' in your scene yet.");
            return;
        }
        
        handler.Paint(ManageCustomizeSo.Instance.colorDataSo.colors[index].color);
    }
    
    private void ClickWheelItem(int index)
    {
        RCC_CustomizationManager handler = RCC_CustomizationManager.Instance;
        
        if (!handler) {
            Debug.LogError("You are trying to customize the vehicle, but there is no ''RCC_CustomizationManager'' in your scene yet.");
            return;
        }
        Debug.Log(index);
        handler.ChangeWheels(index);
    }
}
