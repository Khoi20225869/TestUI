using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Modal;
using System.Collections;

public class EntryPoint : MonoBehaviour
{
    public PageContainer _pageContainer;
    
    private void Start()
    {
        StartCoroutine(PushGaragePage());
    }

    private IEnumerator PushGaragePage()
    {
        yield return _pageContainer.Push("GaragePage9", true);
    }
}
