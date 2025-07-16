using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;
using System.Collections;

public class EntryPoint : MonoBehaviour
{
    public PageContainer _pageContainer;
    
    public void Start()
    {
        StartCoroutine(ShowMainPage());
    }

    IEnumerator ShowMainPage()
    {
        Debug.Log(">> Bắt đầu Push MainPage");
        yield return _pageContainer.Push("GaragePage9", true);
        Debug.Log(">> MainPage đã được hiển thị");
    }
}
