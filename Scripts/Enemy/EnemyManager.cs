using System;
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
    [HideInInspector] public float timeLimit;

    // Serialize
    [SerializeField] EnemyDataList enemyDataList;

    [Header("スポーン座標")]
    [SerializeField] GameObject spawnVolume;

    [Header("次のエネミーが出てくるまでのインターバル")]
    [SerializeField] float spawnInterval=1.0f;

    [Header("エネミーが逃げるまでのインターバル")]
    public float runInterval = 1.0f;

    //
    private CancellationTokenSource cts = new CancellationTokenSource();


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

    void Start()
    {
        SpawnProcess().Forget();
    }


    // ===========================================
    // EnemyManagerの初期化
    // ===========================================
    public void Init()
    {
        if (!cts.Token.IsCancellationRequested)
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }
        waveCount = 1;
        currentEnemyCount = 0;
        if (currentEnemy != null)
        {
            Destroy(currentEnemyObj);
        }
    }


    async UniTask SpawnProcess()
    {
        while (cts.Token.CanBeCanceled)
        {
            if (cts.Token.IsCancellationRequested) { break; }
            Debug.Log("aaa");
            await UniTask.WaitUntil(() => currentEnemyCount < 1);

            if (currentEnemyCount < 1)
            {
                currentEnemyCount++;
                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval + UnityEngine.Random.Range(-0.5f, 0.5f)));
                await SpawnNextEnemy();
            }

            await UniTask.WaitWhile(() => currentEnemyCount < 1);
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

        await spawner.SpawnT(enemyData.prefabAddress, MS_Random.GetRandomPositionInSpawnVolume(spawnVolume),false);
        currentEnemyObj = spawner.prefabs;

        // Enemyコンポーネント取得
        currentEnemy = currentEnemyObj.GetComponent<Enemy>();
        currentEnemy.data = enemyData;

        //エネミーコンポーネントを初期化
        currentEnemy.runInterval = runInterval;
        currentEnemy.waveCount = waveCount;
        currentEnemy.Initialize();
        currentEnemyObj.SetActive(true);

    }

    public void Reset()
    {
        Init();
        SpawnProcess().Forget();
    }
}
