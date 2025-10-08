using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CookieInfo : MonoBehaviour
{

    public TMPro.TextMeshProUGUI cookieText;


    void Update()
    {
        cookieText.text = "Cookies: " + CookieManager.Instance.cookies;
    }
}
