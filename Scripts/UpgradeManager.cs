using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public UpgradeDataList upgradeDataList;
    public CookieManager cookieManager;

    private int[] purchaseCounts;

    void Start()
    {
        purchaseCounts = new int[upgradeDataList.upgrades.Length];
    }

    public void TryBuyUpgrade(int index)
    {
        var data = upgradeDataList.upgrades[index];
        int level = purchaseCounts[index];
        int cost = data.baseCost * (level + 1);

        if (cookieManager.cookies >= cost)
        {
            cookieManager.cookies -= cost;
            cookieManager.cookiesPerSecond += data.cpsIncrease;
            purchaseCounts[index]++;
        }
    }

    public int GetUpgradeLevel(int index)
    {
        if (index >= 0 && index < purchaseCounts.Length)
            return purchaseCounts[index];
        return 0;
    }
}
