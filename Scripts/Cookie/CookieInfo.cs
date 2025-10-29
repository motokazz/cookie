using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CookieInfo : MonoBehaviour
{
    public TextMeshProUGUI cookieText;
    public TextMeshProUGUI cpsText;
    public TextMeshProUGUI cpcText;

    CookieManager cm;

    private void Start()
    {
        cm = GameManager.Instance.cookieManager;
    }
    void Update()
    {
        cookieText.text = "Cookies: " + cm.cookies;
        cpsText.text = $"CPS : {cm.cookiesPerSecond.ToString("F1")}";
        cpcText.text = $"CPC : {cm.cookiesPerClick}";
    }
}
