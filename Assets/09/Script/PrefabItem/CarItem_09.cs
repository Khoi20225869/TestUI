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

        int unlocked = PlayerData.GetCarUnlockStatus(index);
        bool checkPrice = vehicle.collectType == VehicleCollectType.Free;

        if (checkPrice)
        {
            unlocked = 1;
            PlayerData.SetCarUnlockStatus(index);
        }

        _lockIcon.SetActive(unlocked != 1);
        _lockBg.SetActive(unlocked != 1);

        _carItemBtn.onClick.RemoveAllListeners();
        _carItemBtn.onClick.AddListener(() => _onSelected?.Invoke(_index));
        
        _onSelected += OnSelected;
        _originalScale = transform.localScale;
    }

    private void OnDestroy()
    {
        _carItemBtn.onClick.RemoveAllListeners();
        _scaleTween.Kill();
    }

    private void OnSelected(int index)
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale * 1.2f, 0.3f).SetEase(Ease.OutBack);
        GaragePage_09._currentItem = this;
        if (PlayerData.GetCarUnlockStatus(index) == 1)
            PlayerData.SetCurrentCarIndex(index);

        VehicleManager.Instance.SpawnVehicle(index, false);
    }
    
    public void Unselect()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale, 0.25f).SetEase(Ease.InOutSine);
    }

    public void Open(int index)
    {
        _lockIcon.SetActive(false);
        _lockBg.SetActive(false);
        PlayerData.SetCarUnlockStatus(index);
        PlayerData.SetCurrentCarIndex(index);
    }
    
    public void OnSelectedImmediate()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originalScale * 1.2f, 0.3f).SetEase(Ease.OutBack);
    }
}    

