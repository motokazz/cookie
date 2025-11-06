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
        textMeshPro.text = gm.enemyManager.timeLimit.ToString();
    }

}
