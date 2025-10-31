using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミー攻撃ボタン
/// </summary>
/// 

public class EnemyAttackButton : MonoBehaviour
{

    [SerializeField] Button clickButton;

    // Private
    CookieManager cookieManager;
    EnemyManager enemyManager;

    private void Awake()
    {
        cookieManager = GameManager.Instance.cookieManager;
        enemyManager = GameManager.Instance.enemyManager;
    }

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
