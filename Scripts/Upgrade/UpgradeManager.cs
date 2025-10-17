using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public UpgradeDataList upgradeDataList_org;//元のUpgradeDataList

    [Header("アップグレードコスト倍率")]
    [SerializeField] float upgradeCostRatio = 1.15f;
    [SerializeField] float cpsIncreaseRatio = 1.15f;

    [HideInInspector]
    public UpgradeDataList upgradeDataList;//ゲーム中に使うUpgradeDataList


    void Awake()
    {
        upgradeDataList = Instantiate (upgradeDataList_org);
        InitUpgradeDataList();
    }

    private void Start()
    {
        InitUpgradeDataList();
    }

    public void InitUpgradeDataList()
    {
        for (int i = 0; i< upgradeDataList.upgrades.Count;i++)
        {
            var data = upgradeDataList.upgrades[i];
            upgradeDataList.upgrades[i].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[i].cpsIncreaseTotal = CPSCalc(data.cpsIncrease,data.level);
        }
    }

    int CostCalc(int baseCost, int level)
    {
        float temp = baseCost * MathF.Pow(upgradeCostRatio , level);
        return (int)temp;
    }

    float CPSCalc(float cpsIncrease, int level)
    {
        float temp = cpsIncrease * level * cpsIncreaseRatio;
        return temp;
    }

    public void TryBuyUpgrade(int index)
    {
        var data = upgradeDataList.upgrades[index];
        int cost = data.currentCost;

        if (CookieManager.Instance.cookies >= cost)
        {
            upgradeDataList.upgrades[index].level++;
            //コスト更新
            upgradeDataList.upgrades[index].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[index].cpsIncreaseTotal = CPSCalc(data.cpsIncrease, data.level);
            Debug.Log(upgradeDataList.upgrades[index].level);

            CookieManager.Instance.cookies -= cost;
            CookieManager.Instance.cookiesPerSecond += data.cpsIncrease * cpsIncreaseRatio;
        }
    }

    public void ArrayToLevels(int[] upgradeLevels)
    {
        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            upgradeDataList.upgrades [i].level = upgradeLevels [i];
        }
    }

    public int[] LevelsToArray()
    {
        List<int> temp = new List<int>();
        for (int i = 0; i < upgradeDataList.upgrades.Count; i++)
        {
            temp.Add(upgradeDataList.upgrades[i].level);
        }
        return temp.ToArray();
    }
}
