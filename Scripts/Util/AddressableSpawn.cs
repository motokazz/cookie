using UnityEngine;
using UnityEngine.AddressableAssets;
// ===========================================
// Addressableに登録されているGameObjectをSpawnする
// コンポーネント化したくないのでMonoBehavior継承しないようにしてる
// ===========================================
public class AddressableSpawn
{
    public GameObject gameObject;
    
    public GameObject spawn(string key)
    {
        SpawnAsync(key);
        return gameObject;
    }

    public async void SpawnAsync(string key)
    {
        key = key.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");//改行削除
        var handle = Addressables.LoadAssetAsync<GameObject>(key);
        await handle.Task;
        MonoBehaviour.Instantiate(handle.Result);
        gameObject = handle.Result;
    }

}
