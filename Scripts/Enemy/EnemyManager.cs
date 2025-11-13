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
    [HideInInspector] public float timeLimit;

    // Serialize
    [SerializeField] EnemyDataList enemyDataList;

    [Header("スポーン座標")]
    [SerializeField] GameObject spawnVolume;

    [Header("次のエネミーが出てくるまでのインターバル")]
    [SerializeField] float spawnInterval=1.0f;

    [Header("エネミーが逃げるまでのインターバル")]
    public float runInterval = 1.0f;


   
    // Private
    private GameObject currentEnemyObj;
    private Spawner spawner;
    private bool spawnReady = false;
    

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
        // 敵がいなくなったら発生準備
        if (currentEnemyCount < 1)
        {
            spawnReady = true;
        }
        else
        {
            spawnReady = false;
        }

        //　準備できてたら敵を発生
        if (spawnReady) {
            currentEnemyCount++;
            await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval + UnityEngine.Random.Range(-0.5f, 0.5f)));
            await SpawnNextEnemy();
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
        currentEnemy.Initialize(waveCount,runInterval);
    }


}
