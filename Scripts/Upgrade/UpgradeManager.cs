using System;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    [SerializeField] UpgradeDataList upgradeDataList_org;//元のUpgradeDataList
    [HideInInspector] public UpgradeDataList upgradeDataList;//ゲーム中に使うUpgradeDataList


    [Header("アップグレードコスト倍率")]
    [SerializeField] float upgradeCostRatio = 1.15f;
    [SerializeField] float cpsIncreaseRatio = 1.15f;

    NPCSpawn nPCSpawn;

    void Awake()
    {
        Init();
        
    }


    // ===========================================
    // UpgradeDataList初期化
    // ===========================================

    public void Init()
    {
        upgradeDataList = Instantiate(upgradeDataList_org);
        for (int i = 0; i< upgradeDataList.upgrades.Count;i++)
        {
            var data = upgradeDataList.upgrades[i];
            upgradeDataList.upgrades[i].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[i].cpsIncreaseTotal = CPSCalc(data.cpsIncrease,data.level);
        }
    }


    // ===========================================
    // アップグレード購入
    // ===========================================

    public void TryBuyUpgrade(int index)
    {
        var gm = GameManager.Instance;
        var data = upgradeDataList.upgrades[index];
        var cost = data.currentCost;

        if (gm.cookieManager.cookies >= cost)
        {
            upgradeDataList.upgrades[index].level++;
            //コスト更新
            upgradeDataList.upgrades[index].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[index].cpsIncreaseTotal = CPSCalc(data.cpsIncrease, data.level);

            gm.cookieManager.cookies -= cost;
            gm.cookieManager.cookiesPerSecond += data.cpsIncrease * cpsIncreaseRatio;

            //NPC作成
            string key = upgradeDataList.upgrades[index].prefab;
            NPCSpawn nPCSpawn = new NPCSpawn();
            nPCSpawn.Spawn(key);
        }
    }


    // ===========================================
    // 計算
    // ===========================================

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


}
