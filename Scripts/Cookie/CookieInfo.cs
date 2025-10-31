using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ゲーム情報表示
/// </summary>
/// 
public class CookieInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cookieText;
    [SerializeField] TextMeshProUGUI cpsText;
    [SerializeField] TextMeshProUGUI cpcText;
    [SerializeField] TextMeshProUGUI waveText;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }
    void Update()
    {
        // 現在のクッキー
        ChangeText(cookieText, "Cookies: " + gm.cookieManager.cookies);

        // 現在のトータルCPS
        ChangeText(cpsText, $"CPS : {gm.cookieManager.cookiesPerSecond.ToString("F1")}");

        // 現在のトータルCPC
        ChangeText(cpcText, $"CPC : {gm.cookieManager.cookiesPerClick}");

        // 現在のWaveCount
        ChangeText(waveText, $"CPC : {gm.enemyManager.waveCount}");

    }

    // ===========================================
    // TMP存在確認＆テキスト内容書き換え
    // ===========================================
    void ChangeText(TextMeshProUGUI tmp,string txt)
    {
        if (tmp != null) { 
            tmp.text = txt;
        }
    }
}
