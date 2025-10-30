using System;
using UnityEngine;


public class UpgradeManager : MonoBehaviour
{
    [SerializeField] UpgradeDataList upgradeDataList_org;//元のUpgradeDataList
    [HideInInspector] public UpgradeDataList upgradeDataList;//ゲーム中に使うUpgradeDataList


    [Header("アップグレードコスト倍率")]
    [SerializeField] float upgradeCostRatio = 1.15f;
    [SerializeField] float cpsIncreaseRatio = 1.15f;

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

    public async void TryBuyUpgrade(int index)
    {
        var cookieManager = GameManager.Instance.cookieManager;
        var upgrade = GameManager.Instance.upgradeManager.upgradeDataList.upgrades[index];
        var cost = upgrade.currentCost;

        if (cookieManager.cookies >= cost)
        {
            // アップグレードレベル更新
            upgrade.level++;

            //コスト更新
            upgrade.currentCost = CostCalc(upgrade.baseCost, upgrade.level);
            upgrade.cpsIncreaseTotal = CPSCalc(upgrade.cpsIncrease, upgrade.level);

            cookieManager.cookies -= cost;
            cookieManager.cookiesPerSecond += upgrade.cpsIncrease * cpsIncreaseRatio;

            //NPC作成
            string key = upgradeDataList.upgrades[index].prefab;
            var prefab = await AddressableSpawn.SpawnAsync(key);
            // モデルスポーン
            //GameObject gameObject = prefab.gameObject;
            //Instantiate(prefab.gameObject);

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
