using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
public class ModePage : Page
{
    [SerializeField] private Button _backBtn;
    [Header("References")]
    [SerializeField] private GameObject _modeButtonPrefab;
    [SerializeField] private Transform _content;

    [Header("Data")]
    [SerializeField] private ModePageSoData _soData;

    public override IEnumerator Initialize()
    {
        if (_backBtn != null) _backBtn.onClick.RemoveAllListeners();

        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        foreach (var modeEntry in _soData.Modes)
        {
            var btnGo = Instantiate(_modeButtonPrefab, _content);
            var image = btnGo.gameObject.GetComponent<Image>();
            var button = btnGo.GetComponent<Button>();

            image.sprite = modeEntry.icon;
            button.onClick.RemoveAllListeners();

            GameMode chosen = modeEntry.mode;

            button.onClick.AddListener(() =>
            {
                StartCoroutine(
                    PageContainer.Of(transform)
                        .Push(
                            "LevelPage",
                            true,
                            onLoad: handle =>
                            {
                                var page = handle.page as LevelPage;
                                page.SetupWithMode(modeEntry);
                            }
                        )
                );
            });
        }

        if (_backBtn != null)
        {
            _backBtn.onClick.AddListener(() =>
            {
                StartCoroutine(
                    PageContainer.Of(transform)
                        .Pop(true)
                );
            });
        }

        yield break;
    }
}
