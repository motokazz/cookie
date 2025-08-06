using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CookieManager : MonoBehaviour
{
    public int cookies = 0;
    public float cookiesPerSecond = 0f;

    public TMPro.TextMeshProUGUI cookieText;
    public EnemyManager enemyManager; // ← 追加！
    [SerializeField] Button clickButton;

    private float cookieBuffer = 0f;

    private void Start()
    {
        clickButton.onClick.AddListener(() => OnClickCookie());
    }
    void Update()
    {
        cookieBuffer += cookiesPerSecond * Time.deltaTime;

        if (cookieBuffer >= 1f)
        {
            int add = Mathf.FloorToInt(cookieBuffer);
            cookies += add;
            cookieBuffer -= add;
            enemyManager.currentEnemy.TakeDamage(add); // ここで攻撃
        }

        cookieText.text = "Cookies: " + cookies;
    }

    public void OnClickCookie()
    {
        cookies++;

        // クリックと同時に攻撃処理
        if (enemyManager != null && enemyManager.currentEnemy != null)
        {
            enemyManager.currentEnemy.TakeDamage(1); // ここで攻撃
        }
    }
}
