using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
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
    [SerializeField] private int _ROW    = 3;
    [SerializeField] private int _COLUMN = 4;
    private    int pageSize => _ROW * _COLUMN;

    [Header("UI")]
    [SerializeField] private Button _backBtn;

    private ObjectPool<LevelItem_09> _pool;
    private int _totalLevel;
    private int _totalPage;

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
        _totalPage  = Mathf.CeilToInt((float)_totalLevel / pageSize);
        
        for (int i = 0; i < _totalPage; i++)
        {
            var index = i;
            _dynamic.Add(index);  
        }

        _scrollSnap.OnPanelSelected.RemoveAllListeners();
        _scrollSnap.OnPanelSelected.AddListener(OnPanelChanged);

        _scrollSnap.GoToPanel(0);
    }

    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnLevelItems(0));
    }

    private void OnPanelChanged(int panelIndex)
    {
        StartCoroutine(SpawnLevelItems(panelIndex));
    }

    private IEnumerator SpawnLevelItems(int panelIndex)
    {
 
        RectTransform panel   = _scrollSnap.Panels[panelIndex];
        Debug.Log(panel);
        Transform     content = panel.Find("PageLevelItem");
        if (content == null)
        {
            Debug.LogError("Panel prefab phải có child tên 'Content' để chứa các nút level");
            yield break;
        }
        
        foreach (Transform child in panel)
        {
            if (child.TryGetComponent<LevelItem_09>(out var item))
                _pool.Release(item);
        }


        int start = panelIndex * pageSize;
        int end   = Mathf.Min(start + pageSize, _totalLevel);
        
        for (int i = start; i < end; i++)
        {
            var item = _pool.Get();
            item.transform.SetParent(panel, false);
            
            item.Init(i);
            item._button.onClick.RemoveAllListeners();
            int idx = i; 
            item._button.onClick.AddListener(() => OnLevelClicked(idx));

            yield return new WaitForSeconds(0.05f);
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
