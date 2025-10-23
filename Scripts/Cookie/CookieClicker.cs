using UnityEngine;
using UnityEngine.UI;


public class CookieClicker : MonoBehaviour
{
    public CookieManager cookieManager;
    [SerializeField] Button clickButton;

    private void Start()
    {
        clickButton.onClick.AddListener(() => OnClickCookie());
    }

    public void OnClickCookie()
    {
        cookieManager.cookies++;
    }
}
