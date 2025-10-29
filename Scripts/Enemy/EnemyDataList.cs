using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewEnemyDataList", menuName = "Game/Enemy Data List")]
[System.Serializable]
public class EnemyDataList : ScriptableObject
{
    public List<EnemyData> enemyList;
}
