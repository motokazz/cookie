using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public CookieManager cookieManager;

    public void Save()
    {
        PlayerPrefs.SetInt("cookies", cookieManager.cookies);
        PlayerPrefs.SetFloat("cps", cookieManager.cookiesPerSecond);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        cookieManager.cookies = PlayerPrefs.GetInt("cookies", 0);
        cookieManager.cookiesPerSecond = PlayerPrefs.GetFloat("cps", 0f);
    }
}
