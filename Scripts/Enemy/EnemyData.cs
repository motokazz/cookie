using UnityEngine;

/// <summary>
/// エネミー基本データ
/// </summary>

[System.Serializable]
public class EnemyData
{
    // エネミー名
    public string enemyName = "";

    // MaxHP
    public double maxHP = 100;

    // 倒した時の賞金
    public double rewardCookies = 0;
    
    // エネミーPrefab定義
    public string prefabAddress = "";
}
