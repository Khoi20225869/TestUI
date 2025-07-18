using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class SelectModePage_09 : Page
{
    [SerializeField] private GameObject _modeButtonPrefab;
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _taskBtn;
    [SerializeField] private Transform _content;
    [SerializeField] private ModePageSoData _soData;

    public override IEnumerator Initialize()
    {
        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(OnBackClicked);
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
        StartCoroutine(SpawnModes());
    }
    
    
    private IEnumerator SpawnModes()
    {
        foreach (var mode in _soData.Modes)
        {
            var btn = Instantiate(_modeButtonPrefab, _content);
            var image = btn.gameObject.GetComponent<Image>();
            var button = btn.GetComponent<Button>();
            
            image.sprite = mode.icon;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClickMode(mode));
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private void OnBackClicked()
    {
        Debug.Log("Nhấn Back!");
        StartCoroutine(PageContainer.Of(transform).Pop(true));
        VehicleManager.Instance.SpawnVehicle(PlayerData.GetCurrentCarIndex(), false);
    }

    private void OnClickMode(ModePageSoData.Mode mode)
    {
        Debug.Log("Nhấn Click!");
        StartCoroutine(PageContainer.Of(transform).Push("LevelModePage9", true, onLoad : handle =>
        {
            var page = handle.page as LevelPage_09;
            page.SetupWithMode(mode);
        }));
    }
    private void OnTaskSelected()
    {
        StartCoroutine(PageContainer.Of(transform).Push("MissionModal9", true));
    }

}
