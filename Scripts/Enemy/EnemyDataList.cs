using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミー基本データリスト
/// </summary>
/// 

[CreateAssetMenu(fileName = "NewEnemyDataList", menuName = "Game/Enemy Data List")]
[System.Serializable]
public class EnemyDataList : ScriptableObject
{
    public List<EnemyData> enemyList;
}
