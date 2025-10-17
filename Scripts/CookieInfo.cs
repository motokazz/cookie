using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CookieInfo : MonoBehaviour
{

    public TextMeshProUGUI cookieText;
    public TextMeshProUGUI cpsText;
    public TextMeshProUGUI cpcText;


    void Update()
    {
        cookieText.text = "Cookies: " + CookieManager.Instance.cookies;
        cpsText.text = $"CPS : {CookieManager.Instance.cookiesPerSecond.ToString("F1")}";
        cpcText.text = $"CPC : {CookieManager.Instance.cookiesPerClick}";
    }
}
