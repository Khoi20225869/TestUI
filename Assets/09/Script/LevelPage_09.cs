using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityScreenNavigator.Runtime.Core.Page;

public class LevelPage_09 : Page
{
    [Header("ScrollSnap & Pagination")]
    [SerializeField] private SimpleScrollSnap _scrollSnap;
    [SerializeField] private DynamicContent _dynamic;

    [Header("Panel & Level Button Prefabs")]
    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private GameObject _levelButtonPrefab;

    [Header("Layout Settings")]
    [SerializeField] private int _ROW = 2;
    [SerializeField] private int _COLUMN = 6;
    private int pageSize => _ROW * _COLUMN;

    [Header("UI")]
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _taskBtn;

    [Header("Effect")]
    [SerializeField] private float spawnButtonDelay = 0.07f; 

    private int _totalLevel;
    private int _totalPage;
    private Dictionary<int, Transform> _pageContents = new Dictionary<int, Transform>();

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

    public void SetupWithMode(ModePageSoData.Mode mode)
    {
        _totalLevel = mode.totalLevel;
        _totalPage = Mathf.CeilToInt((float)_totalLevel / pageSize);

        for (int i = 0; i < _totalPage; i++)
        {
            var panelGO = _dynamic.Add(i);
            if (panelGO == null)
            {
                Debug.LogError("Không lấy được panel từ _dynamic.Add(i)!");
                continue;
            }
            var content = panelGO.transform.Find("Content");
            if (content == null)
            {
                Debug.LogError($"Panel {panelGO.name} không có child 'Content'!");
                continue;
            }
            _pageContents[i] = content;
        }

        for(int i = 0; i < _totalPage;i++) StartCoroutine(SpawnLevelItems(i));

        _scrollSnap.GoToPanel(0);
    }

    private IEnumerator SpawnLevelItems(int panelIndex)
    {
        if (!_pageContents.TryGetValue(panelIndex, out var content))
        {
            Debug.LogError($"Không tìm thấy content cho panelIndex {panelIndex}");
            yield break;
        }

        int start = panelIndex * pageSize;
        int end = Mathf.Min(start + pageSize, _totalLevel);

        for (int i = start; i < end; i++)
        {
            var go = Instantiate(_levelButtonPrefab, content);
            var item = go.GetComponent<LevelItem_09>();
            if (item == null)
            {
                Debug.LogError("Prefab _levelButtonPrefab không có component LevelItem_09!");
                continue;
            }
            item.Init(i);

            int idx = i;
            if (item._button != null)
            {
                item._button.onClick.RemoveAllListeners();
                item._button.onClick.AddListener(() => OnLevelClicked(idx));
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
