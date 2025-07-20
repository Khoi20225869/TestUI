using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class SelectModePage : Page
{
    [SerializeField] private GameObject modeButtonPrefab;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button taskBtn;
    [SerializeField] private Transform content;
    [SerializeField] private ModePageSoData soData;

    public override IEnumerator Initialize()
    {
        if (backBtn != null)
        {
            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(OnBackClicked);
        }

        if (taskBtn != null)
        {
            taskBtn.onClick.RemoveAllListeners();
            taskBtn.onClick.AddListener(OnTaskSelected);
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
        foreach (var mode in soData.Modes)
        {
            var btn = Instantiate(modeButtonPrefab, content);
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
            var page = handle.page as LevelPage;
            if (page != null) page.SetupWithMode(mode);
        }));
    }
    private void OnTaskSelected()
    {
        StartCoroutine(PageContainer.Of(transform).Push("MissionModal9", true));
    }

}
