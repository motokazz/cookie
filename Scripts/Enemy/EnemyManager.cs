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
    // Enemyの初期化 渡したエネミーデータで上書き
    // ===========================================
    void Initialize(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentHP = currentEnemy.data.maxHP * waveCount;

        //UI
        if (currentEnemy.hpText != null) currentEnemy.hpText.text = $"HP: {currentEnemy.currentHP}";
        if (currentEnemy.nameText != null) currentEnemy.nameText.text = currentEnemy.data.enemyName;
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

        //逃走用タスク
        //await RunProcess();
    }

    // ===========================================
    // スポーン間隔調整
    // ===========================================
    CancellationTokenSource cts = new CancellationTokenSource();
    public async UniTask SpawnProcess()
    {
        
        CancellationToken token = cts.Token;

        if (currentEnemyObj == null)
        {
            await ShowWaitTime(spawnInterval);
            await SpawnNextEnemy();
        }
        if(currentEnemyObj != null)
        {
            await RunProcess(token);
        }
        else
        {
            cts.Cancel();
        }

    }

    async UniTask RunProcess(CancellationToken token)
    {
        if (currentEnemyObj != null)
        {
            await ShowWaitTime(runInterval);
            await Run();
        }
    }

    private async UniTask ShowWaitTime(float seconds)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            timeLimit=remaining;
            await UniTask.Yield(); // 次のフレームまで待つ
            remaining -= Time.deltaTime;
        }
    }



    // ===========================================
    // エネミー挙動
    // ===========================================
    // ダメージ処理
    public async UniTask TakeDamage(int damage)
    {
        if (currentEnemyObj != null)
        {
            currentEnemy.currentHP -= damage;
            if (currentEnemy.currentHP > 0)
            {
                if (currentEnemy.hpText != null) currentEnemy.hpText.text = $"HP: {currentEnemy.currentHP}";
            }
            else
            {
                await Die();
            }
        }
    }

    // 逃走
    async UniTask Run()
    {
        Destroy(currentEnemyObj);
        cts.Cancel();
        await SpawnProcess();
    }

    // 死亡
    async UniTask Die()
    {
        //勝利ボーナス
        GameManager.Instance.cookieManager.cookies += currentEnemy.data.rewardCookies;
        
        Destroy(currentEnemyObj);

        waveCount++;

        //Spawn
        cts.Cancel();
        await SpawnProcess();
        
    }
}
