using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
public class LevelPage : Page
{
    [SerializeField] private Button _backBtn;
    [Header("UI References")]
    [SerializeField] private Transform _levelContentContainer;
    [SerializeField] private GameObject _levelButtonPrefab;

    private ModePageSoData.Mode _mode;

    public void SetupWithMode(ModePageSoData.Mode mode)
    {
        _mode = mode;

        if (_backBtn != null)
        {
            _backBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.AddListener(() =>
            {
                StartCoroutine(
                    PageContainer.Of(transform)
                        .Pop(true)
                );
            });
        }
    }

    public override void DidPushEnter()
    {
        base.DidPushEnter();
        StartCoroutine(SpawnButtons());
    }


    private IEnumerator SpawnButtons()
    {
        foreach (Transform c in _levelContentContainer) Destroy(c.gameObject);

        for (int i = 1; i <= _mode.totalLevel; i++)
        {
            var go = Instantiate(_levelButtonPrefab, _levelContentContainer);
            var btn = go.GetComponent<Button>();

            int level = i;
            string sceneAddress = _mode.levelSceneAddresses[level - 1];
            btn.onClick.AddListener(() =>
            {
                /*StartCoroutine(LoadMapScene(sceneAddress));*/
                OnLevelButtonClicked(sceneAddress).Forget();

            });
            yield return null;  
        }
    }


    async UniTaskVoid OnLevelButtonClicked(string sceneAddr)
    {
        await LoadingManager.Instance.LoadSceneAsync(sceneAddr);
    }
    /*private IEnumerator LoadMapScene(string sceneAddress)
    {
        var handle = Addressables.LoadSceneAsync(
            sceneAddress,
            LoadSceneMode.Single,
            activateOnLoad: true
        );
        yield return handle;
    }*/
}
