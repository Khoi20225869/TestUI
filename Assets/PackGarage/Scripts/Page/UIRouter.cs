using System.Collections;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;

public class UIRouter : MonoBehaviour
{
    PageContainer _pageContainer;

    void Awake()
    {
        _pageContainer = PageContainer.Of(transform);
    }

    void Start()
    {
        StartCoroutine(ShowMainPage());
    }

    IEnumerator ShowMainPage()
    {
        Debug.Log(">> Bắt đầu Push MainPage");
        yield return _pageContainer.Push("MainPage", true);
        Debug.Log(">> MainPage đã được hiển thị");
    }
}
