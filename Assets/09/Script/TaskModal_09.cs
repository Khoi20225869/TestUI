using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Page = UnityScreenNavigator.Runtime.Core.Page.Page;

public class TaskModal_09 : Page
{
    [SerializeField] private Button _backBtn;
    [SerializeField] private AchievementDataSo _soData;
    [SerializeField] private GameObject _taskItemPrefab;
    [SerializeField] private Transform _content;
    
    
    public override IEnumerator Initialize()
    {
        if(_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(OnBackButtonClicked);
        }

        yield break;
    }

    private void OnBackButtonClicked()
    {
        foreach (Transform child in _content)
            DOTween.Kill(child);
        StopAllCoroutines();
        StartCoroutine(PageContainer.Of(transform).Pop(true));
    }

    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnTasks());
    }

    private IEnumerator SpawnTasks()
    {
        foreach (var mission in _soData.missions)
        {
            var go = Instantiate(_taskItemPrefab, _content);
            if (go == null || go.transform == null)
                continue;
            
            go.transform.localScale = Vector3.one * 0.7f;
            string objName = go.name;
            go.transform.DOScale(1f, 0.45f)
                .SetEase(Ease.OutBack)
                .SetTarget(go.transform)
                .SetAutoKill(true);
            
            var item = go.GetComponent<TaskItem_09>();
            if (item == null) item.AddComponent<TaskItem_09>();
            item.SetUp(mission);
    
            yield return new WaitForSeconds(0.2f);
        }
    }

}
