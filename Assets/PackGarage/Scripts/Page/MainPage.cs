using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using System.Collections;

public class MainPage : Page
{
    [SerializeField] private Button _garageBtn;
    [SerializeField] private Button _playBtn;

    public override IEnumerator Initialize()
    {
        _garageBtn.onClick.RemoveAllListeners();
        _playBtn.onClick.RemoveAllListeners();

        _garageBtn.onClick.AddListener(() =>
        {
            StartCoroutine(
        PageContainer.Of(transform)
            .Push(
                "GaragePage",  
                true,        
                onLoad : handle =>     
                {
                    var page = handle.page as GaragePage;
                    page.SetupWithData();
                }
            )
    );
        });

        _playBtn.onClick.AddListener(() =>
        {
            StartCoroutine(PageContainer
                .Of(transform)
                .Push("ModePage", true));
        });


        yield break;
    }
}
