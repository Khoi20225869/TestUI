using System.Collections.Generic;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityEngine;
using UnityScreenNavigator.Runtime.Foundation.Coroutine;

public class ExamplePage : MonoBehaviour
{
    private PageContainer _pageContainer;
    public IEnumerable<AsyncProcessHandle> OnButtonClick()
    {
        var handle = _pageContainer.Push("ExamplePage", true);
        yield return handle;
    }
}
