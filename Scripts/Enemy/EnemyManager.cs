using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField]　public EnemyDataList enemyDataList;

    [Header("スポーン座標")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject spawnVolume;

    [Header("エネミーデータが無かった時使うプレファブ")]
    [SerializeField] GameObject fallbackEnemyPrefab;

    [Header("次のエネミーが出てくるまでのインターバル")]
    [SerializeField] float spawnInterval=1.0f;


    //
    [NonSerialized] public Enemy currentEnemy;
    [NonSerialized] public int waveCount = 1;


    private GameObject currentEnemyObj;
    private Coroutine coroutine;


    private void Awake()
    {
        currentEnemy =gameObject.AddComponent<Enemy>();
    }


    // ===========================================
    // スポーン処理
    // ===========================================

    public void SpawnNextEnemy()
    {

        if (enemyDataList == null || enemyDataList.enemyList.Length == 0)
        {
            Debug.LogWarning("EnemyDataList is empty!");
            return;
        }

        // 敵を順番にまたはランダムに選出
        int index = (waveCount - 1) % enemyDataList.enemyList.Length;
        index = 0;
        EnemyData selected = enemyDataList.enemyList[index];

        // 敵プレファブ見つからなかったら予備プレファブ
        GameObject prefab = selected.enemyPrefab != null ? selected.enemyPrefab : fallbackEnemyPrefab;

        // スポーン
        Vector3 spawnPos = spawnPoint != null ? GetRandomPositionInSpawnVolume() : Vector3.zero;
        currentEnemyObj = Instantiate(prefab, spawnPos , Quaternion.identity);

        // Enemyコンポーネント取得
        currentEnemy = currentEnemyObj.GetComponent<Enemy>();

        //エネミーコンポーネントを初期化
        Initialize(selected);

    }
    
    
    IEnumerator WaitSpawnNext()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnNextEnemy();
    }

    // volume内の点をランダムに抽出
    Vector3 GetRandomPositionInSpawnVolume()
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
    
    // EnemyData初期化
    void Initialize(EnemyData enemyData)
    {
        currentEnemy.data = enemyData;
        currentEnemy.currentHP = currentEnemy.data.maxHP*waveCount;

        //UI
        if (currentEnemy.hpText != null) currentEnemy.hpText.text = $"HP: {currentEnemy.currentHP}";
        if (currentEnemy.nameText != null) currentEnemy.nameText.text = currentEnemy.data.enemyName;
    }


    // ダメージ処理
    public void TakeDamage(int damage)
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
                Die();
            }
        }
    }


    // 死亡
    void Die()
    {
        //勝利ボーナス
        GameManager.Instance.cookieManager.cookies += currentEnemy.data.rewardCookies;
        
        Destroy(currentEnemyObj);

        waveCount++;
        
        //Spawn
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(WaitSpawnNext());
        
    }
}
