﻿using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]　EnemyDataList enemyDataList; // ← ScriptableObject で複数管理

    [Header("スポーン座標")]
    [SerializeField] Transform spawnPoint;

    [Header("エネミーデータが無かった時使うプレファブ")]
    [SerializeField] GameObject fallbackEnemyPrefab;


    [NonSerialized] public Enemy currentEnemy;
    [NonSerialized] public int waveCount = 1;

    private GameObject currentEnemyObj;
    private Coroutine coroutine;


    public void Start()
    {
        SpawnNextEnemy();
    }


    public void SpawnNextEnemy()
    {


        if (enemyDataList == null || enemyDataList.enemyList.Length == 0)
        {
            Debug.LogWarning("EnemyDataList is empty!");
            return;
        }

        // 敵を順番にまたはランダムに選出
        int index = (waveCount - 1) % enemyDataList.enemyList.Length;
        EnemyData selected = enemyDataList.enemyList[index];

        // 敵プレファブ見つからなかったら予備プレファブ
        GameObject prefab = selected.enemyPrefab != null ? selected.enemyPrefab : fallbackEnemyPrefab;

        // スポーン
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        currentEnemyObj = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Enemyコンポーネント取得
        currentEnemy = currentEnemyObj.GetComponent<Enemy>();

        //エネミーコンポーネントを初期化
        Initialize(selected);
    }


    // EnemyData初期化
    void Initialize(EnemyData enemyData)
    {
        currentEnemy.data = enemyData;
        currentEnemy.currentHP = currentEnemy.data.maxHP;

        //UI
        if (currentEnemy.hpText != null) currentEnemy.hpText.text = $"HP: {currentEnemy.currentHP}";
        if (currentEnemy.nameText != null) currentEnemy.nameText.text = currentEnemy.data.enemyName;
    }


    // ダメージ処理
    public void TakeDamage(int damage)
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


    // 死亡
    void Die()
    {
        Debug.Log("Die");
        //勝利ボーナス
        CookieManager.Instance.cookies += currentEnemy.data.rewardCookies;
        
        Destroy(currentEnemyObj);

        waveCount++;

        SpawnNextEnemy();
        /*
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(WaitSpawnNext());
        */
    }

    public void ResetEnemy()
    {
        waveCount = 1;
        Destroy(currentEnemyObj);
        SpawnNextEnemy();
    }

    IEnumerator WaitSpawnNext()
    {
        yield return new WaitForSeconds(2f);
        SpawnNextEnemy();
    }

}
