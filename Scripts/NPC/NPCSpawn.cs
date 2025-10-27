using UnityEngine;
using UnityEngine.AddressableAssets;

public class NPCSpawn:MonoBehaviour
{

    public void Spawn(string key)
    {
        SpawnAsync(key);
    }

    async void SpawnAsync(string key)
    {
        key = key.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");//���s�폜
        var handle = Addressables.LoadAssetAsync<GameObject>(key);
        await handle.Task;
        Instantiate(handle.Result);
    }

}
