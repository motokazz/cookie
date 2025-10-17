using UnityEngine;
using UnityEngine.UI;


public class EnemyAttackButton : MonoBehaviour
{

    public CookieManager cookieManager;
    public EnemyManager enemyManager; // ← 追加！
    [SerializeField] Button clickButton;


    private void Start()
    {
        clickButton.onClick.AddListener(() => OnClickCookie());
    }

    public void OnClickCookie()
    {
        // クリックと同時に攻撃処理
        if (enemyManager != null && enemyManager.currentEnemy != null)
        {
            enemyManager.TakeDamage(Mathf.FloorToInt(cookieManager.cookiesPerClick)); // ここで攻撃
        }
    }
}
