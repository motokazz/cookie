using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] CookieManager cookieManager;

    [SerializeField] UpgradeDataList upgradeDataList_org;//元のUpgradeDataList
    [HideInInspector] public UpgradeDataList upgradeDataList;//ゲーム中に使うUpgradeDataList


    [Header("アップグレードコスト倍率")]
    [SerializeField] float upgradeCostRatio = 1.15f;
    [SerializeField] float cpsIncreaseRatio = 1.15f;

    //Addressable読み込み用
    AsyncOperationHandle<GameObject> operationHandle;


    void Awake()
    {
        upgradeDataList = Instantiate (upgradeDataList_org);
        InitUpgradeDataList();
    }


    // ===========================================
    // UpgradeDataList初期化
    // ===========================================

    public void InitUpgradeDataList()
    {
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
        var data = upgradeDataList.upgrades[index];
        var cost = data.currentCost;

        if (cookieManager.cookies >= cost)
        {
            upgradeDataList.upgrades[index].level++;
            //コスト更新
            upgradeDataList.upgrades[index].currentCost = CostCalc(data.baseCost, data.level);
            upgradeDataList.upgrades[index].cpsIncreaseTotal = CPSCalc(data.cpsIncrease, data.level);

            cookieManager.cookies -= cost;
            cookieManager.cookiesPerSecond += data.cpsIncrease * cpsIncreaseRatio;

            //NPC作成
            string key = upgradeDataList.upgrades[index].prefab;
            Debug.Log(key);
            //key = "human";
            operationHandle = Addressables.LoadAssetAsync<GameObject>(key);
            if (operationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject obj = operationHandle.Result;
                Instantiate(obj, transform);
            }
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
