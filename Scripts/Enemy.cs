using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public int currentHP;

    public EnemyManager enemyManager;
    public Canvas worldSpaceCanvas;
    public TMPro.TextMeshProUGUI hpText;
    public TMPro.TextMeshProUGUI nameText;

    public void Initialize(EnemyData enemyData, EnemyManager manager)
    {
        data = enemyData;
        enemyManager = manager;
        currentHP = data.maxHP;

        if (hpText != null) hpText.text = $"HP: {currentHP}";
        if (nameText != null) nameText.text = data.enemyName;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            if (hpText != null) hpText.text = $"HP: {currentHP}";
        }
    }

    void Die()
    {
        enemyManager.cookieManager.cookies += data.rewardCookies;
        enemyManager.SpawnNextEnemy();
        Destroy(gameObject);
    }
}
