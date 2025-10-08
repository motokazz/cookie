using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public EnemyDataList enemyDataList; // ← ScriptableObject で複数管理
    public Transform spawnPoint;
    public GameObject fallbackEnemyPrefab;//エネミーデータが無かった時使うプレファブ

    public Enemy currentEnemy;
    public int waveCount = 0;

    public void Start()
    {
        SpawnNextEnemy();
    }

    public void SpawnNextEnemy()
    {
        waveCount++;

        if (enemyDataList == null || enemyDataList.enemyList.Length == 0)
        {
            Debug.LogWarning("EnemyDataList is empty!");
            return;
        }

        // 敵を順番にまたはランダムに選出
        int index = (waveCount - 1) % enemyDataList.enemyList.Length;
        EnemyData selected = enemyDataList.enemyList[index];

        GameObject prefab = selected.enemyPrefab != null
            ? selected.enemyPrefab
            : fallbackEnemyPrefab;

        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        currentEnemy = obj.GetComponent<Enemy>();
        currentEnemy.Initialize(selected, this);
    }
}
