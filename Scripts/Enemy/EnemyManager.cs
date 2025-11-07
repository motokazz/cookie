using System;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


/// <summary>
/// エネミーマネージャー
/// </summary>
/// 

public class EnemyManager : MonoBehaviour
{
    // Public
    [HideInInspector] public int waveCount = 1;
    [HideInInspector] public Enemy currentEnemy;
    [HideInInspector] public int currentEnemyCount = 0;

    // Serialize
    [SerializeField] EnemyDataList enemyDataList;

    [Header("スポーン座標")]
    [SerializeField] GameObject spawnVolume;

    [Header("次のエネミーが出てくるまでのインターバル")]
    [SerializeField] float spawnInterval=1.0f;

    [Header("エネミーが逃げるまでのインターバル")]
    public float runInterval = 1.0f;
    public float timeLimit;

    // Private
    private GameObject currentEnemyObj;

    private Spawner spawner;


    private void Awake()
    {
        if (GetComponent<Spawner>() == null)
        {
            spawner = gameObject.AddComponent<Spawner>();
        }
    }

    // ===========================================
    // EnemyManagerの初期化
    // ===========================================

    public void Init()
    {
        waveCount = 1;
        if (currentEnemy != null)
        {
            Destroy(currentEnemyObj);
        }
    }

    public async void Update()
    {
        if (currentEnemyCount <= 0) {
            currentEnemyCount++;
            await SpawnProcess();
        }
    }

    // ===========================================
    // スポーン処理
    // ===========================================

    public async UniTask SpawnNextEnemy()
    {
        // EnemyDataListチェック
        if (enemyDataList == null || enemyDataList.enemyList.Count == 0)
        {
            Debug.LogWarning("EnemyDataList is empty!");
            return;
        }

        // 敵を順番にまたはランダムに選出
        int index = (waveCount - 1) % enemyDataList.enemyList.Count;
        EnemyData enemyData = enemyDataList.enemyList[index];

        // モデルスポーン
        await spawner.Spawn(enemyData.prefabAddress, spawnVolume);
        currentEnemyObj = spawner.prefabs;

        // Enemyコンポーネント取得
        currentEnemy = currentEnemyObj.GetComponent<Enemy>();
        currentEnemy.data = enemyData;

        //エネミーコンポーネントを初期化
        currentEnemy.Initialize(waveCount);
        
        cts.Cancel();// SpawnProcessキャンセル

        await currentEnemy.RunProcess(runInterval);
    }

    // ===========================================
    // スポーン間隔調整
    // ===========================================
    CancellationTokenSource cts;
    public async UniTask SpawnProcess()
    {
        cts = new CancellationTokenSource();

        if (currentEnemyObj == null)
        {
            await ShowWaitTime(spawnInterval,cts.Token);
            await SpawnNextEnemy();
        }

    }

    private async UniTask ShowWaitTime(float seconds,CancellationToken token)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            timeLimit=remaining;
            await UniTask.Yield(token); // 次のフレームまで待つ
            remaining -= Time.deltaTime;
        }
    }
}
