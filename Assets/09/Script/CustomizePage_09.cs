using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;

public class CustomizePage_09 : Page
{
    [SerializeField] private ManageCustomizeSo _soData;
    [SerializeField] private Button _backBtn;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _optionPrefab;
    [SerializeField] private Transform _optionContent;
    [SerializeField] private Transform _itemContent;
    public override IEnumerator Initialize()
    {
        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(OnBackButtonClicked);
        }
        
        yield break;
    }
    
    public override void DidPushEnter()
    {
        base.DidPushEnter();
        //StartCoroutine(SpawnOptions());
    }

    /*private IEnumerator SpawnOptions()
    {
        foreach (Transform child in _optionContent)
            Destroy(child.gameObject);

        for (int i = 0; i < _soData.totalOption; i++)
        {
            var go = Instantiate(_optionPrefab, _optionContent);
            
            yield return new WaitForSeconds(0.2f);
        }
    }*/
    private void OnBackButtonClicked()
    {
        StopAllCoroutines();
        StartCoroutine(PageContainer.Of(transform).Pop(false));
    }
}
