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
    [SerializeField] private int _ROW = 2; // 2 hàng
    [SerializeField] private int _COLUMN = 6; // 6 cột
    private int pageSize => _ROW * _COLUMN;

    [Header("UI")]
    [SerializeField] private Button _backBtn;

    private int _totalLevel;
    private int _totalPage;
    private Dictionary<int, Transform> _pageContents = new Dictionary<int, Transform>();
    private HashSet<int> _spawnedPages = new HashSet<int>();

    public override IEnumerator Initialize()
    {
        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(OnBackClicked);
        }
        yield break;
    }

    public void SetupWithMode(ModePageSoData.Mode mode)
    {
        _totalLevel = mode.totalLevel;
        _totalPage = Mathf.CeilToInt((float)_totalLevel / pageSize);

        _pageContents.Clear();
        _spawnedPages.Clear();

        // Tạo panel và lưu Content
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

        _scrollSnap.OnPanelSelected.RemoveAllListeners();
        _scrollSnap.OnPanelSelected.AddListener(TrySpawnLevelItems);

        // Sinh page đầu
        TrySpawnLevelItems(0);
        _scrollSnap.GoToPanel(0);
    }
    
    private void TrySpawnLevelItems(int panelIndex)
    {
        // 1. Luôn spawn cho panel được chọn
        if (!_spawnedPages.Contains(panelIndex))
        {
            StartCoroutine(SpawnLevelItems(panelIndex));
            _spawnedPages.Add(panelIndex);
        }

        // 2. Spawn trước cho panel trước và sau (preload)
        int[] preload = { panelIndex - 1, panelIndex + 1 };
        foreach (var i in preload)
        {
            if (i >= 0 && i < _totalPage && !_spawnedPages.Contains(i))
            {
                StartCoroutine(SpawnLevelItems(i));
                _spawnedPages.Add(i);
            }
        }

        // 3. XÓA các page KHÔNG phải page hiện tại hoặc hai page liền kề
        for (int i = 0; i < _totalPage; i++)
        {
            if (i == panelIndex || i == panelIndex - 1 || i == panelIndex + 1)
                continue; // giữ lại 3 page này

            if (_spawnedPages.Contains(i))
            {
                // Xóa toàn bộ button của page i
                if (_pageContents.TryGetValue(i, out var content))
                {
                    for (int j = content.childCount - 1; j >= 0; j--)
                    {
                        Destroy(content.GetChild(j).gameObject);
                    }
                }
                _spawnedPages.Remove(i);
            }
        }
    }


    [Header("Effect")]
    [SerializeField] private float spawnButtonDelay = 0.07f; // xuất hiện từng cái

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
}
