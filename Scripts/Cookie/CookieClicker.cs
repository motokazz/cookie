using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// クッキー増やすボタン用
/// </summary>

public class CookieClicker : MonoBehaviour
{
    [SerializeField] Button clickButton;

    private void Start()
    {
        clickButton.onClick.AddListener(() => OnClickCookie());
    }

    public void OnClickCookie()
    {
        GameManager.Instance.cookieManager.cookies++;
    }
}
