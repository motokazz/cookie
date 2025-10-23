using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewEnemyDataList", menuName = "Game/Enemy Data List")]
public class EnemyDataList : ScriptableObject
{
    public EnemyData[] enemyList;
}
