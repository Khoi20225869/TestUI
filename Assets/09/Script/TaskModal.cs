using System.Collections;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Page = UnityScreenNavigator.Runtime.Core.Page.Page;

public class TaskModal : Page
{
    [SerializeField] private Button backBtn;
    [SerializeField] private AchievementDataSo soData;
    [SerializeField] private GameObject taskItemPrefab;
    [SerializeField] private Transform content;
    
    
    public override IEnumerator Initialize()
    {
        if(backBtn != null)
        {
            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(OnBackButtonClicked);
        }

        yield break;
    }

    private void OnBackButtonClicked()
    {
        foreach (Transform child in content)
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
        foreach (var mission in soData.missions)
        {
            var go = Instantiate(taskItemPrefab, content);
            if (go == null || go.transform == null)
                continue;
            
            go.transform.localScale = Vector3.one * 0.7f;
            go.transform.DOScale(1f, 0.45f)
                .SetEase(Ease.OutBack)
                .SetTarget(go.transform)
                .SetAutoKill(true);
            
            var item = go.GetComponent<TaskItem>();
            if (item == null) item.AddComponent<TaskItem>();
            item.SetUp(mission);
    
            yield return new WaitForSeconds(0.2f);
        }
    }

}
