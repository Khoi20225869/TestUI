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
    [SerializeField] private Button _taskBtn;


    public static CarItem_09 _currentItem;
    public override IEnumerator Initialize()
    {
        if (_playBtn != null)
        {
            _playBtn.onClick.RemoveAllListeners();
            _playBtn.onClick.AddListener(OnPlayClicked);
        }

        if (_customizeBtn != null)
        {
            _customizeBtn.onClick.RemoveAllListeners();
            _customizeBtn.onClick.AddListener(OnCustomizeClicked);
        }

        if (_taskBtn != null)
        {
            _taskBtn.onClick.RemoveAllListeners();
            _taskBtn.onClick.AddListener(OnTaskSelected);
        }

        yield break;
    }


    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnItems());
    }
    
    public override void DidPopEnter()
    {
        base.DidPopEnter();
        StartCoroutine(SpawnItems()); 
    }

    private IEnumerator SpawnItems()
    {
        foreach (Transform c in _carContent)
            Destroy(c.gameObject);

        CarItem_09 firstItem = null;
        for (int i = 0; i < _soData.vehicles.Length; i++)
        {
            var vehicle = _soData.vehicles[i];
            var go = Instantiate(_carItemPrefab, _carContent);
            var item = go.GetComponent<CarItem_09>();
            item.Instantiate(vehicle, i, OnItemSelected);

            if (i == PlayerData.GetCurrentCarIndex()) firstItem = item;
            yield return new WaitForSeconds(0.2f);
        }
        
        if (firstItem != null)
        {
            _currentItem = firstItem;
            firstItem.OnSelectedImmediate();
        }
    }


    private void OnPlayClicked()
    {
        StopAllCoroutines();
        foreach (Transform c in _carContent)
            Destroy(c.gameObject);
        StartCoroutine(PageContainer.Of(transform).Push("SelectModePage9", true));
    }

    private void OnCustomizeClicked()
    {
        StopAllCoroutines();
        foreach (Transform c in _carContent)
            Destroy(c.gameObject);
        StartCoroutine(PageContainer.Of(transform).Push("CustomizePage9", false));
    }
    
    private void OnItemSelected(int index)
    {
        _currentItem?.Unselect();
    }

    private void OnTaskSelected()
    {
        StopAllCoroutines();
        foreach (Transform c in _carContent)
            Destroy(c.gameObject);
        StartCoroutine(PageContainer.Of(transform).Push("MissionModal9", true));
    }
}
