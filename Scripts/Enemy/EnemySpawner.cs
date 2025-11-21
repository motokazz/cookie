using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class EnemySpawnera : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float spawnInterval = 2f;

    CancellationTokenSource cts;

    void OnEnable()
    {
        cts = new CancellationTokenSource();
        SpawnLoopAsync(cts.Token).Forget();
    }

    void OnDisable()
    {
        cts?.Cancel();
        cts?.Dispose();
    }

    async UniTaskVoid SpawnLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            SpawnEnemy();
            await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        var point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, point.position, point.rotation);
    }
}
