using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CookieInfo : MonoBehaviour
{
    [SerializeField] CookieManager cookieManager;
    public TextMeshProUGUI cookieText;
    public TextMeshProUGUI cpsText;
    public TextMeshProUGUI cpcText;


    void Update()
    {
        cookieText.text = "Cookies: " + cookieManager.cookies;
        cpsText.text = $"CPS : {cookieManager.cookiesPerSecond.ToString("F1")}";
        cpcText.text = $"CPC : {cookieManager.cookiesPerClick}";
    }
}
