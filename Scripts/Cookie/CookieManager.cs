using System;
using UnityEngine;

public class CookieManager : MonoBehaviour
{
    public double cookies = 0;
    public float cookiesPerSecond = 0f;
    public float cookiesPerClick = 1f;

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
}
