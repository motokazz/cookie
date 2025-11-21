using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;


/// <summary>
/// エネミースポーナー　ObjectPool対応
/// </summary>

public class EnemyWaveSpawnerAddressable : MonoBehaviour
{
    [Header("ランダム入りスポーン座標")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 spawnPointRandomMin = Vector3.zero;
    [SerializeField] private Vector3 spawnPointRandomMax = Vector3.zero;

    [Header("スポーン間隔調整")]
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float waveInterval = 5f;

    [Header("オブジェクトリスト")]
    [SerializeField] public EnemyDataList enemies;

    // オブジェクトプール
    private List<ObjectPoolAddressable> objectPoolAddressables = new List<ObjectPoolAddressable>();
    private PooledObjectAddressable po;

    private CancellationTokenSource cts;
    


    private void Awake()
    {
        Init();
    }

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

    // オブジェクトプール初期化
    private void Init()
    {
        foreach (EnemyData enemyData in enemies.enemyList)
        {
            var opa = gameObject.AddComponent<ObjectPoolAddressable>();
            opa.key = enemyData.prefabAddress;
            opa.initPoolSize = 10;
            opa.SetupPool();
            objectPoolAddressables.Add(opa);
        }
    }

    // 逃走処理
    async UniTaskVoid RunWavesAsync(CancellationToken token)
    {
        int wave = 0;
        
        while (!token.IsCancellationRequested)
        {
            wave++;
            Debug.Log($"Wave {wave} start");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                var id = MS_Shuffle.Shuffle(0, enemies.enemyList.Count-1);
               
                po = await objectPoolAddressables[id].GetPooledObject();

                po.transform.position = spawnPoint.position + MS_Random.Vector3Random(spawnPointRandomMin,spawnPointRandomMax);

                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval), cancellationToken: token);

            }

            Debug.Log($"Wave {wave} complete");
            await UniTask.Delay(TimeSpan.FromSeconds(waveInterval), cancellationToken: token);
        }
    }
}
