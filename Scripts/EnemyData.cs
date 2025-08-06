using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP;
    public int rewardCookies;

    public GameObject enemyPrefab; // �� Sprite�ł͂Ȃ�Prefab�i3D���f���j
}
