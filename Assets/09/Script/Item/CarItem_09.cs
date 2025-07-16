using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CarItem_09 : MonoBehaviour
{
    [SerializeField] private Button _carItemBtn;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _lockBg;
    [SerializeField] private Image _carIcon;
    
    private Tween _scaleTween;
    private Vector3 _originalScale;
    
    private int _index;
    public void Instantiate(VehicleDataSo.Vehicle vehicle, int index, Action<int> _onSelected)
    {
        _carIcon.sprite = vehicle.vehicleImage;
        _index = index;

        bool unlocked = PlayerPrefs.GetInt(vehicle.vehicleSaveName, 0) == 1;
        bool checkPrice = vehicle.collectType == VehicleCollectType.Free;

        if (checkPrice)
        {
            unlocked = true;
            PlayerPrefs.SetInt(vehicle.vehicleSaveName, 1);
        }

        _lockIcon.SetActive(!unlocked);
        _lockBg.SetActive(!unlocked);

        _carItemBtn.onClick.RemoveAllListeners();
        
        _carItemBtn.onClick.AddListener(() => _onSelected?.Invoke(_index));
        
        _onSelected += OnSelected;
        _originalScale = transform.localScale;
    }

    private void OnSelected(int index)
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale * 1.2f, 0.3f).SetEase(Ease.OutBack);
        GaragePage_09._currentItem = this;
    }
    
    public void Unselect()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale, 0.25f).SetEase(Ease.InOutSine);
    }

    public void Open()
    {
        _lockIcon.SetActive(false);
        _lockBg.SetActive(false);
    }
}    

