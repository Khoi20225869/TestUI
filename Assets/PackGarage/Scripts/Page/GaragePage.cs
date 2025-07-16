using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;

public class GaragePage : Page
{
    [Header("Data")]
    [SerializeField] private VehicleDataSo _soData;         
    [SerializeField] private GameObject _carItemPrefab;  
    [SerializeField] private Transform _carContent;


    [Header("Panel Buttons")]
    [SerializeField] private Button _purchaseBtn;
    [SerializeField] private Button _customizeBtn;
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _backBtn;

    private int _selectedIndex = 0;
    private VehicleDataSo.Vehicle _selectedVehicle;

    public void SetupWithData()
    {
        _selectedIndex = 0;
        _selectedVehicle = null;

        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(() =>
            {
                StartCoroutine(PageContainer.Of(transform).Pop(true));
            });
        }
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
            var v = _soData.vehicles[i];
            var go = Instantiate(_carItemPrefab, _carContent);
            var item = go.GetComponent<CarItem>();
            item.Setup(v, i, OnCarSelected);

            yield return new WaitForSeconds(0.2f);
        }

        _purchaseBtn.gameObject.SetActive(false);
        _customizeBtn.gameObject.SetActive(false);
        _playBtn.gameObject.SetActive(false);

        int initial = PlayerPrefs.GetInt(KeySave.Instance.playerPrefsPlayerVehicle, 0);
        OnCarSelected(initial);
    }

    private void OnCarSelected(int index)
    {
        /*if (_selectedVehicle != null)
        {
            Destroy(RCCP_SceneManager.Instance.activePlayerVehicle);
            _selectedVehicle = null;
        }*/
        if (_selectedIndex >= 0)
        {
            var prev = _carContent.GetChild(_selectedIndex).GetComponent<CarItem>();
            prev.SetHighlight(false);
        }

        var currItem = _carContent.GetChild(index).GetComponent<CarItem>();
        currItem.SetHighlight(true);

        _selectedIndex = index;
        _selectedVehicle = _soData.vehicles[index];

        /*RCCP.SpawnRCC(_selectedVehicle.vehicle, Vector3.zero, Quaternion.identity, false, false, true);*/

        string key = _selectedVehicle.vehicleSaveName;
        bool unlocked = PlayerPrefs.GetInt(key, 0) == 1;
        bool checkPrice = _selectedVehicle.collectType == VehicleCollectType.Free;

        if(checkPrice) unlocked = true;

        _purchaseBtn.gameObject.SetActive(!unlocked);
        _customizeBtn.gameObject.SetActive(unlocked);
        _playBtn.gameObject.SetActive(unlocked);

        _purchaseBtn.onClick.RemoveAllListeners();
        _customizeBtn.onClick.RemoveAllListeners();
        _playBtn.onClick.RemoveAllListeners();

        if (!unlocked)
        {
            _purchaseBtn.onClick.AddListener(() =>
            {
                currItem.SetLockIcon();
                PlayerPrefs.SetInt(key, 1);
                PlayerPrefs.SetInt(KeySave.Instance.playerPrefsPlayerVehicle, index);
                PlayerPrefs.Save();
                OnCarSelected(index); 
            });
        }
        else
        {
            _customizeBtn.onClick.AddListener(() =>
            {
                StartCoroutine(
                    PageContainer.Of(transform)
                        .Push("CustomizePage", true, onLoad : handle =>
                        {
                            var page = handle.page as CustomizePage;
                            page.SetupWithVehicle(_selectedVehicle);
                        })
                );
            });

            _playBtn.onClick.AddListener(() =>
            {
                StartCoroutine(
                    PageContainer.Of(transform)
                    .Push("ModePage", true));
            });
        }
    }
}
