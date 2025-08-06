using UnityEngine;

public class EnemyAutoAttack : MonoBehaviour
{
    public CookieManager cookieManager;
    public EnemyManager enemyManager;

    private float attackBuffer = 0f;

    void Update()
    {
        if (enemyManager.currentEnemy == null) return;
        if (cookieManager.cookies <= 0) return;

        attackBuffer += cookieManager.cookiesPerSecond * Time.deltaTime;

        if (attackBuffer >= 1f)
        {
            int attacks = Mathf.FloorToInt(attackBuffer);
            int possibleAttacks = Mathf.Min(attacks, cookieManager.cookies);

            for (int i = 0; i < possibleAttacks; i++)
            {
                enemyManager.currentEnemy.TakeDamage(1);
            }

            attackBuffer -= possibleAttacks;
        }
    }
}
