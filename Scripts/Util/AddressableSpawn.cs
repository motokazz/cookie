using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Linq;


// ===========================================
// Addressableに登録されているGameObjectをSpawnする
// コンポーネント化したくないのでMonoBehavior継承しないようにしてる
// ===========================================

/// <summary>
/// Addressables から生成したオブジェクトを自動で解放する仕組み。
/// Destroy 時に ReleaseInstance が自動で呼ばれる。
/// </summary>
public static class AddressableSpawn
{
    // 存在確認
    public static async UniTask<bool> Exists(string pathToAsset)
    => (await Addressables.LoadResourceLocationsAsync(pathToAsset)).Any();

    //Spawn
    public static async Task<GameObject> SpawnAsync(string key, Transform parent = null)
    {
        key = key.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");//改行削除

        // 存在確認
        var exists = await Exists(key);

        if (exists)
        {
            // 非同期で生成
            var handle = Addressables.InstantiateAsync(key, parent);
            var instance = await handle.Task;

            // 自動解放用コンポーネントを追加
            var autoReleaser = instance.AddComponent<AddressableAutoRelease>();
            autoReleaser.SetHandle(handle);
            return instance;
        }
        else
        {
            Debug.LogWarning($"Failed to instantiate Addressable: {key}");
            return null;
        }
    }
}




/// <summary>
/// このコンポーネントが付いている GameObject が Destroy された時に
/// Addressables.ReleaseInstance(handle) を自動で呼ぶ。
/// </summary>
public class AddressableAutoRelease : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> handle;
    private bool valid = false;

    public void SetHandle(AsyncOperationHandle<GameObject> handle)
    {
        this.handle = handle;
        valid = handle.IsValid();
    }

    private void OnDestroy()
    {
        if (valid && handle.IsValid())
        {
            Addressables.ReleaseInstance(handle);
            valid = false;
        }
    }
}
