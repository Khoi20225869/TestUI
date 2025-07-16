using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async UniTask LoadSceneAsync(string sceneAddress)
    {
        var handle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Single, activateOnLoad: false);

        while (!handle.IsDone)
        {
            await UniTask.Yield();
        }
        await handle.Result.ActivateAsync().ToUniTask();
    }
}
