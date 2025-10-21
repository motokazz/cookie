using System;
using System.Collections.Generic;
using System.Numerics;
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

    // UpgradeDataList初期化
    public void InitUpgradeDataList()
    {
        for (int i = 0; i< upgradeDataList.upgrades.Count;i++)
        {
            var data = upgradeDataList.upgrades[i];
            upgradeDataList.upgrades[i].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[i].cpsIncreaseTotal = CPSCalc(data.cpsIncrease,data.level);
        }
    }
    // コスト計算
    int CostCalc(double baseCost, int level)
    {
        var temp = baseCost * MathF.Pow(upgradeCostRatio, level);
        return (int)temp;
    }

    // CPS計算
    float CPSCalc(float cpsIncrease, int level)
    {
        float temp = cpsIncrease * level * cpsIncreaseRatio;
        return temp;
    }

    // アップグレード購入
    public void TryBuyUpgrade(int index)
    {
        var data = upgradeDataList.upgrades[index];
        var cost = data.currentCost;

        if (CookieManager.Instance.cookies >= cost)
        {
            upgradeDataList.upgrades[index].level++;
            //コスト更新
            upgradeDataList.upgrades[index].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[index].cpsIncreaseTotal = CPSCalc(data.cpsIncrease, data.level);

            CookieManager.Instance.cookies -= cost;
            CookieManager.Instance.cookiesPerSecond += data.cpsIncrease * cpsIncreaseRatio;
        }
    }

    // ==============================================
    // セーブデータ用
    // ==============================================

    // アップグレードレベル配列をupgradeDataListに代入
    public void ArrayToLevels(int[] upgradeLevels)
    {
        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            upgradeDataList.upgrades [i].level = upgradeLevels [i];
        }
    }

    // UpgradeDataListのレベルを配列に変換
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
