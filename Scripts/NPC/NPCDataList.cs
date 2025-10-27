using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDataList", menuName = "Game/NPC Data List")]
[System.Serializable]
public class NPCDataList : ScriptableObject

{
    public List<NPCData> npcList;
}
