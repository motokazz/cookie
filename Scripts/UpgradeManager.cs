using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public UpgradeDataList upgradeDataList;

    public int[] purchaseCounts;

    void Start()
    {
        purchaseCounts = new int[upgradeDataList.upgrades.Length];
    }

    public void TryBuyUpgrade(int index)
    {
        var data = upgradeDataList.upgrades[index];
        var level = purchaseCounts[index];
        int cost = data.baseCost * (level + 1);

        if (CookieManager.Instance.cookies >= cost)
        {
            CookieManager.Instance.cookies -= cost;
            CookieManager.Instance.cookiesPerSecond += data.cpsIncrease;
            purchaseCounts[index]++;
        }
    }
}
