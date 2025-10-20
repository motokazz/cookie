using System.Numerics;
using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public string upgradeName;
    public string description;
    
    public double baseCost;
    public double currentCost;

    public float cpsIncrease;
    public float cpsIncreaseTotal;

    public int level;
}
