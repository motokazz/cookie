using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/// <summary>
/// エネミースポーナー　ObjectPool対応
/// </summary>

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Vector3 spawnPointRandomMin = Vector3.zero;
    [SerializeField] Vector3 spawnPointRandomMax = Vector3.zero;
    [SerializeField] int enemiesPerWave = 5;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float waveInterval = 5f;
    [SerializeField] ObjectPool pool;

    CancellationTokenSource cts;
    PooledObject po;

    void OnEnable()
    {
        cts = new CancellationTokenSource();
        RunWavesAsync(cts.Token).Forget();
    }

    void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    async UniTaskVoid RunWavesAsync(CancellationToken token)
    {
        int wave = 0;
        
        while (!token.IsCancellationRequested)
        {
            wave++;
            Debug.Log($"Wave {wave} start");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                po = pool.GetPooledObject();
                po.transform.position = spawnPoint.position + Vector3Random(spawnPointRandomMin,spawnPointRandomMax);
                po.transform.rotation = UnityEngine.Random.rotation;

                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);
            }

            Debug.Log($"Wave {wave} complete");
            await UniTask.Delay(TimeSpan.FromSeconds(waveInterval), cancellationToken: token);
        }
    }

    Vector3 Vector3Random(Vector3 min,Vector3 max)
    {
        Vector3 ret = Vector3.zero;
        ret.x = UnityEngine.Random.Range(min.x, max.x);
        ret.y = UnityEngine.Random.Range(min.y, max.y);
        ret.z = UnityEngine.Random.Range(min.z, max.z);
        return ret;
    }
}
