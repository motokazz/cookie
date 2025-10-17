using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;
    
    public int baseCost;
    public int currentCost;

    public float cpsIncrease;
    public float cpsIncreaseTotal;

    public int level;
}
