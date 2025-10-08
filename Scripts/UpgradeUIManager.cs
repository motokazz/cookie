using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UpgradeUIManager : MonoBehaviour
{
    public UpgradeDataList upgradeDataList;
    public UpgradeManager upgradeManager;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    private List<Button> buttons = new List<Button>();
    private List<TextMeshProUGUI> labels = new List<TextMeshProUGUI>();

    void Start()
    {
        GenerateButtons();
    }

    void Update()
    {
        UpdateButtonStates(); // 毎フレーム更新
    }

    void GenerateButtons()
    {
        for (int i = 0; i < upgradeDataList.upgrades.Length; i++)
        {
            UpgradeData data = upgradeDataList.upgrades[i];
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            buttonObj.name = $"UpgradeButton_{i}";

            buttonObj.gameObject.SetActive(false);

            TextMeshProUGUI label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            Button btn = buttonObj.GetComponent<Button>();

            int capturedIndex = i;
            btn.onClick.AddListener(() => upgradeManager.TryBuyUpgrade(capturedIndex));

            buttons.Add(btn);
            labels.Add(label);
        }
    }

    void UpdateButtonStates()
    {
        for (int i = 0; i < upgradeDataList.upgrades.Length; i++)
        {
            var data = upgradeDataList.upgrades[i];
            var level = upgradeManager.purchaseCounts[i];
            int cost = data.baseCost * (level + 1);

            // ラベル更新
            if (labels[i] != null)
            {
                labels[i].text = $"{data.upgradeName}\n{data.description}\nCost: {cost}";
            }

            // ボタンのインタラクティブ更新
            if (buttons[i] != null)
            {
                if (!buttons[i].IsActive() && CookieManager.Instance.cookies >= cost)
                {
                    buttons[i].gameObject.SetActive(true);
                }

                buttons[i].interactable = CookieManager.Instance.cookies >= cost;
            }
        }
    }
}
