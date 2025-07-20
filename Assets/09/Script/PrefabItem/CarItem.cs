using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CarItem : MonoBehaviour
{
    [SerializeField] private Button  carItemBtn;
    [SerializeField] private Image   carIcon;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject lockBg;

    private int _index;
    private Vector3 _originScale;
    private Tween   _scaleTween;

    /// <summary>Khởi tạo 1 item xe.</summary>
    public void Initialize(VehicleDataSo.Vehicle vehicle,
                           int index,
                           Action<int> onSelected)
    {
        _index       = index;
        carIcon.sprite = vehicle.vehicleImage;
        _originScale = transform.localScale;

        bool unlocked = vehicle.collectType == VehicleCollectType.Free ||
                        PlayerData.GetCarUnlockStatus(index) == 1;

        if (vehicle.collectType == VehicleCollectType.Free)
            PlayerData.SetCarUnlockStatus(index);          // unlock ngay lập tức

        lockIcon.SetActive(!unlocked);
        lockBg  .SetActive(!unlocked);

        carItemBtn.onClick.RemoveAllListeners();
        carItemBtn.onClick.AddListener(() => onSelected?.Invoke(_index));
    }

    private void OnDestroy()
    {
        carItemBtn.onClick.RemoveAllListeners();
        _scaleTween?.Kill();
    }

    #region Visual states
    public void SelectAnimated()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originScale * 1.2f, .3f).SetEase(Ease.OutBack);
    }

    public void SelectImmediate()
    {
        _scaleTween?.Kill();
        transform.localScale = _originScale * 1.2f;
    }

    public void Unselect()
    {
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(_originScale, .25f).SetEase(Ease.InOutSine);
    }
    #endregion

    /// <summary>Gọi khi mua xe thành công.</summary>
    public void Unlock()
    {
        lockIcon.SetActive(false);
        lockBg  .SetActive(false);
        PlayerData.SetCarUnlockStatus(_index);
        PlayerData.SetCurrentCarIndex(_index);
    }
}
