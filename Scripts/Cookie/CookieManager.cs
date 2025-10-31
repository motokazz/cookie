using System;
using UnityEngine;

/// <summary>
/// クッキーマネージャー
/// </summary>
public class CookieManager : MonoBehaviour
{
    // 現在のクッキー数
    public double cookies = 0;
    // 秒当たりクッキー生産量
    public float cookiesPerSecond = 0f;
    // クリック当たりクッキー生産量
    public float cookiesPerClick = 1f;

    // Private
    private float cookieBuffer = 0f;//CPS更新用


    void Update()
    {
        cookieBuffer += cookiesPerSecond * Time.deltaTime;

        if (cookieBuffer >= 1f)
        {
            int add = Mathf.FloorToInt(cookieBuffer);
            cookies += add;
            cookieBuffer -= add;
        }
    }

    public void Init()
    {
        cookies = 0;
        cookiesPerSecond = 0f;
        cookiesPerClick = 1f;
    }
}
