using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyDataList", menuName = "Game/Enemy Data List")]
public class EnemyDataList : ScriptableObject
{
    public EnemyData[] enemyList;
}
