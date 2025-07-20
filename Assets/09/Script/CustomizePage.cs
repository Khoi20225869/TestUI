using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class CustomizePage : Page
{
    [SerializeField] private Button backBtn;
    [SerializeField] private Button taskBtn;
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionContent;
    [SerializeField] private Transform itemContent;

    [SerializeField] private Button purchaseByCoinBtn;
    [SerializeField] private Button purchaseByAdsBtn;
    
    public static int CurrentOptionIndex = 0;
    public override IEnumerator Initialize()
    {
        if (backBtn != null)
        {
            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(OnBackButtonClicked);
        }
        
        if (taskBtn != null)
        {
            taskBtn.onClick.RemoveAllListeners();
            taskBtn.onClick.AddListener(OnTaskSelected);
        }

        if (purchaseByCoinBtn != null)
        {
            purchaseByCoinBtn.onClick.RemoveAllListeners();
        }

        if (purchaseByAdsBtn != null)
        {
            purchaseByAdsBtn.onClick.RemoveAllListeners();
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
            var item = go.GetComponent<CustomizeOption>();
            item.Setup(index, itemContent, purchaseByCoinBtn, purchaseByAdsBtn);
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
