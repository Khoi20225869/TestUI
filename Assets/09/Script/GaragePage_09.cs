using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class GaragePage_09 : Page
{
    [Header("Data")]
    [SerializeField] private VehicleDataSo _soData;
    [SerializeField] private GameObject _carItemPrefab;
    [SerializeField] private Transform _carContent;


    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _customizeBtn;
    [SerializeField] private Button _purchaseBtn;
    

    public static CarItem_09 _currentItem;
    public override IEnumerator Initialize()
    {
        if(_playBtn != null)
        {
            _playBtn.onClick.RemoveAllListeners();
            _playBtn.onClick.AddListener(OnPlayClicked);
        }

        if (_customizeBtn != null)
        {
            _customizeBtn.onClick.RemoveAllListeners();
            _customizeBtn.onClick.AddListener(OnCustomizeClicked);
        }

        yield break;
    }


    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnItems());
    }

    private IEnumerator SpawnItems()
    {
        foreach (Transform c in _carContent)
            Destroy(c.gameObject);

        for (int i = 0; i < _soData.vehicles.Length; i++)
        {
            var vehicle = _soData.vehicles[i];
            var go = Instantiate(_carItemPrefab, _carContent);
            var item = go.GetComponent<CarItem_09>();
            item.Instantiate(vehicle, i, OnItemSelected);
            yield return new WaitForSeconds(0.2f);
        }

        /*_purchaseBtn.gameObject.SetActive(false);
        _customizeBtn.gameObject.SetActive(false);
        _playBtn.gameObject.SetActive(false);*/
    }

    private void OnPlayClicked()
    {
        StartCoroutine(PageContainer.Of(transform).Push("SelectModePage9", true));
    }

    private void OnCustomizeClicked()
    {
        Debug.Log("Nhấn Customize! Mở phần tuỳ chỉnh...");
    }
    
    private void OnItemSelected(int index)
    {
        _currentItem?.Unselect();
    }
}
