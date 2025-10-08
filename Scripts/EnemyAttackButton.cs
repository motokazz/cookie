using UnityEngine;
using UnityEngine.UI;


public class EnemyAttackButton : MonoBehaviour
{

    public CookieManager cookieManager;
    public EnemyManager enemyManager; // �� �ǉ��I
    [SerializeField] Button clickButton;


    private void Start()
    {
        clickButton.onClick.AddListener(() => OnClickCookie());
    }

    public void OnClickCookie()
    {
        // �N���b�N�Ɠ����ɍU������
        if (enemyManager != null && enemyManager.currentEnemy != null)
        {
            enemyManager.currentEnemy.TakeDamage(Mathf.FloorToInt(cookieManager.cookiesPerClick)); // �����ōU��
        }
    }
}
