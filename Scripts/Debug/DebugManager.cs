using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;

    private void Update()
    {
        DebugMessages();
    }

    void DebugMessages()
    {
        var gm = GameManager.Instance;
        string tx="";
        tx += gm.enemyManager.timeLimit.ToString() + "\n";
        tx += gm.enemyManager.currentEnemyCount.ToString() + "\n";
        tx += gm.enemyManager.waveCount.ToString() + "\n";

        textMeshPro.text = tx;
            }

}
