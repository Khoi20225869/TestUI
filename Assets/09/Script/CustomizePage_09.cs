using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class CustomizePage_09 : Page
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button _taskBtn;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionContent;
    [SerializeField] private Transform itemContent;
    
    public static int CurrentOptionIndex = 0;
    public override IEnumerator Initialize()
    {
        if (backBtn != null)
        {
            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(OnBackButtonClicked);
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
        StartCoroutine(SpawnOptions());
    }

    private IEnumerator SpawnOptions()
    {
        foreach (Transform child in optionContent)
            Destroy(child.gameObject);

        for (var i = 0; i < ManageCustomizeSo.Instance.totalOption; i++)
        {
            var index = i;
            var go = Instantiate(optionPrefab, optionContent);
            var item = go.GetComponent<CustomizeOption_09>();
            item.Setup(index, itemContent);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnBackButtonClicked()
    {
        StopAllCoroutines();
        StartCoroutine(PageContainer.Of(transform).Pop(true));
    }
    
    private void OnTaskSelected()
    {
        StopAllCoroutines();
        StartCoroutine(PageContainer.Of(transform).Push("MissionModal9", true));
    }
}
