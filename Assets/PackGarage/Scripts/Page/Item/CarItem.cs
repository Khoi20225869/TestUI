using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarItem : MonoBehaviour
{
    [SerializeField] private Image _carImage;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Button _btn;
    [SerializeField] private GameObject _lockIcon;

    private int _index;
    private Action<int> _onSelected;

    public void Setup(VehicleDataSo.Vehicle vehicle, int index, Action<int> onSelected)
    {
        _carImage.sprite = vehicle.vehicleImage;
        _index = index;
        _onSelected = onSelected;

        bool unlocked = PlayerPrefs.GetInt(vehicle.vehicleSaveName, 0) == 1;
        bool checkPrice = vehicle.collectType == VehicleCollectType.Free;

        if (checkPrice) unlocked = true;
        _lockIcon.SetActive(!unlocked);
        _highlight.SetActive(false);


        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() => _onSelected?.Invoke(_index));
    }

    public void SetHighlight(bool active)
    {
        _highlight.SetActive(active);
    }

    public void SetLockIcon()
    {
        _lockIcon.SetActive(false);
    }
}
