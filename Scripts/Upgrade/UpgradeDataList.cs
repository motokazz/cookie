using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewUpgradeDataList", menuName = "Game/Upgrade Data List")]
[Serializable]
public class UpgradeDataList : ScriptableObject
{
    public List<UpgradeData> upgrades;
}
