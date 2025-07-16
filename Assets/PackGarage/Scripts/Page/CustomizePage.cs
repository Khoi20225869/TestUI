using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
public class CustomizePage : Page
{
    [SerializeField] private Button _backBtn;
    public void SetupWithVehicle(VehicleDataSo.Vehicle _selectedVehicle)
    {
        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(() =>
            {
                StartCoroutine(PageContainer.Of(transform).Pop(true));
            });
        }
    }
}
