using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string name;
    public int baseCost = 10;
    public float costMultiplier = 1.15f;
    public float cps = 1f;
    public bool hasBeenRevealed = false;

    [HideInInspector] public int level = 0;

    public int CurrentCost => Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
}
