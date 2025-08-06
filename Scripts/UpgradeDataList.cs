using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgradeDataList", menuName = "Game/Upgrade Data List")]
public class UpgradeDataList : ScriptableObject
{
    public UpgradeData[] upgrades;
}
