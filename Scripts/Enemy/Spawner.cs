using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Addressableをスポーンする
/// </summary>
/// 
public class Spawner:MonoBehaviour
{
    [NonSerialized] public GameObject prefabs;

    // オブジェクト無かった時のプレファブ
    private string fallbackPrefab = "Misc/FallbackCube";

    // ===========================================
    // スポーン処理
    // ===========================================

    public async Task Spawn(string key,GameObject spawnVolume)
    {

        // ===========================================
        // Addressable読み込み
        // ===========================================

        // モデルスポーン
        Vector3 spawnPos = GetRandomPositionInSpawnVolume(spawnVolume);
        prefabs = await AddressableSpawn.SpawnAsync(key);

        // Addressable読めなかったら予備
        if (prefabs == null)
        {
            prefabs = await AddressableSpawn.SpawnAsync(fallbackPrefab);
        }
        prefabs.transform.position = spawnPos;
    }

 
    // volume内の点をランダムに抽出
    Vector3 GetRandomPositionInSpawnVolume(GameObject spawnVolume)
    {
        Vector3 rnd = Vector3.zero;
        if (spawnVolume != null)
        {
            var vol = spawnVolume.GetComponent<MeshFilter>();
            if (vol != null)
            {
                var min = vol.mesh.bounds.min;
                var max = vol.mesh.bounds.max;

                min.x *= spawnVolume.transform.localScale.x;
                min.y *= spawnVolume.transform.localScale.y;
                min.z *= spawnVolume.transform.localScale.z;
                max.x *= spawnVolume.transform.localScale.x;
                max.y *= spawnVolume.transform.localScale.y;
                max.z *= spawnVolume.transform.localScale.z;

                rnd.x = UnityEngine.Random.Range(min.x, max.x);
                rnd.y = UnityEngine.Random.Range(min.y, max.y);
                rnd.z = UnityEngine.Random.Range(min.z, max.z);

            }
        }

        rnd.x += spawnVolume.transform.position.x;
        rnd.y += spawnVolume.transform.position.y;
        rnd.z += spawnVolume.transform.position.z;

        return rnd;
    }

}
