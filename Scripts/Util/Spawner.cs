using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Addressableをスポーンする
/// </summary>
/// await spawner.Spawn(key,gameObject);
/// GameObject go = spawner.prefabs;
/// 

public class Spawner:MonoBehaviour
{
    [NonSerialized] public GameObject prefabs;

    // オブジェクト無かった時のプレファブ
    private string fallbackPrefab = "Misc/FallbackCube";


    // ===========================================
    // スポーン処理
    // ===========================================

    /// <summary>
    /// Addressable読み込み
    /// </summary>
    /// <param name="key">asd</param>
    /// <param name="spawnPos"></param>
    /// <param name="active"></param>
    /// <returns></returns>
    public async Task SpawnT(string key, Vector3 spawnPos,bool active = true)
    {
        // ===========================================
        // Addressable読み込み
        // 
        // ===========================================

        // モデルスポーン
        prefabs = await AddressableSpawn.SpawnAsync(key);

        // Addressable読めなかったら予備
        if (prefabs == null)
        {
            prefabs = await AddressableSpawn.SpawnAsync(fallbackPrefab);

            prefabs.SetActive(active);
        }
        prefabs.transform.position = spawnPos;
    }
}
