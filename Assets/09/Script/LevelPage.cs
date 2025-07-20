using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityScreenNavigator.Runtime.Core.Page;

public class LevelPage : Page
{
    [Header("ScrollSnap & Pagination")]
    [SerializeField] private SimpleScrollSnap scrollSnap;
    [SerializeField] private DynamicContent dynamic;
    
    [Header("Panel & Level Button Prefabs")]
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private GameObject levelButtonPrefab;
    
    [Header("Layout Settings")]
    [SerializeField] private int row = 2;
    [SerializeField] private int column = 6;
    private int PageSize => row * column;
    
    [Header("UI")]
    [SerializeField] private Button backBtn;
    [SerializeField] private Button taskBtn;

    [Header("Effect")]
    [SerializeField] private float spawnButtonDelay = 0.07f; 

    private int _totalLevel;
    private int _totalPage;
    private readonly Dictionary<int, Transform> _pageContents = new Dictionary<int, Transform>();

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

    public void SetupWithMode(ModePageSoData.Mode mode)
    {
        _totalLevel = mode.totalLevel;
        _totalPage = Mathf.CeilToInt((float)_totalLevel / PageSize);

        for (int i = 0; i < _totalPage; i++)
        {
            var panelGo = dynamic.Add(i);
            if (panelGo == null)
            {
                Debug.LogError("Không lấy được panel từ _dynamic.Add(i)!");
                continue;
            }
            var content = panelGo.transform.Find("Content");
            if (content == null)
            {
                Debug.LogError($"Panel {panelGo.name} không có child 'Content'!");
                continue;
            }
            _pageContents[i] = content;
        }

        for(int i = 0; i < _totalPage;i++) StartCoroutine(SpawnLevelItems(i));

        scrollSnap.GoToPanel(0);
    }

    private IEnumerator SpawnLevelItems(int panelIndex)
    {
        if (!_pageContents.TryGetValue(panelIndex, out var content))
        {
            Debug.LogError($"Không tìm thấy content cho panelIndex {panelIndex}");
            yield break;
        }

        int start = panelIndex * PageSize;
        int end = Mathf.Min(start + PageSize, _totalLevel);

        for (int i = start; i < end; i++)
        {
            var go = Instantiate(levelButtonPrefab, content);
            var item = go.GetComponent<LevelItem>();
            if (item == null)
            {
                Debug.LogError("Prefab _levelButtonPrefab không có component LevelItem_09!");
                continue;
            }
            item.Init(i);

            int idx = i;
            if (item.button != null)
            {
                item.button.onClick.RemoveAllListeners();
                item.button.onClick.AddListener(() => OnLevelClicked(idx));
            }
            else
            {
                Debug.LogError("LevelItem_09 prefab thiếu Button reference!");
            }

            yield return new WaitForSeconds(spawnButtonDelay); 
        }
    }

    private void OnLevelClicked(int levelIndex)
    {
        Debug.Log($"Nhấn Level {levelIndex + 1}!");
        // TODO: Load scene hoặc push page tương ứng
    }

    private void OnBackClicked()
    {
        StartCoroutine(PageContainer.Of(transform).Pop(true));
    }

    private void OnTaskSelected()
    {
        StartCoroutine(PageContainer.Of(transform).Push("MissionModal9", true));
    }
}
