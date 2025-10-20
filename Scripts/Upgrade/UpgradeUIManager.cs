using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UpgradeUIManager : MonoBehaviour
{
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

    //ボタン作成
    void GenerateButtons()
    {
        buttons = new List<Button>();
        labels = new List<TextMeshProUGUI>();

        for (int i = 0; i < upgradeManager.upgradeDataList.upgrades.Count; i++)
        {

            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);

            // ボタン
            buttonObj.name = $"UpgradeButton_{i}";
            buttonObj.gameObject.SetActive(false);

            // ボタンコンポーネント取得
            Button btn = buttonObj.GetComponent<Button>();
            buttons.Add(btn);

            // ラベル
            TextMeshProUGUI label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            labels.Add(label);

            // 購買機能追加
            int capturedIndex = i;
            btn.onClick.AddListener(() => upgradeManager.TryBuyUpgrade(capturedIndex));


            //helpメッセージ設定
            UpgradeData data = upgradeManager.upgradeDataList.upgrades[i];
            var helpMessage = buttonObj.GetComponent<HelpMessage>();
            if (helpMessage != null)
            {
                helpMessage.message = data.upgradeName+"\n";
                helpMessage.message += data.baseCost+"\n";
                helpMessage.message += data.cpsIncreaseTotal.ToString("F2")+"\n";
                helpMessage.message += data.description;
            }
        }
    }

    //ボタン更新
    void UpdateButtonStates()
    {
        for (int i = 0; i < upgradeManager.upgradeDataList.upgrades.Count; i++)
        {
            var data = upgradeManager.upgradeDataList.upgrades[i];
            var cost = data.currentCost;
            // ラベル更新
            if (labels[i] != null)
            {
                labels[i].text = $"Lv{data.level} :{data.upgradeName} :cost {cost} :cps+ {data.cpsIncreaseTotal.ToString("F1")}";
            }

            // ボタンの状態更新
            if (buttons[i] != null)
            {
                // ボタンのアクティブ状態
                if (!buttons[i].IsActive())
                {
                    if (data.level > 0 || CookieManager.Instance.cookies >= cost)
                    {
                        buttons[i].gameObject.SetActive(true);
                    }
                }

                // ボタンのインタラクティブ状態
                buttons[i].interactable = CookieManager.Instance.cookies >= cost;
            }

            // ボタンの進捗をゲージ更新
            ButtonValue(buttons[i], (float)cost, (float)CookieManager.Instance.cookies);
        }
    }

    // ボタンの進捗をゲージ更新
    void ButtonValue(Button button,float maxValue,float value)
    {
        var image = button.GetComponent<Image>();
        if (image != null) { image.fillAmount = value / maxValue; }
    }


    // ボタンの状態をリセット
    public void RessetButtons()
    {
        foreach (Transform n in buttonParent)
        {
            Destroy(n.gameObject);
        }
        GenerateButtons();
    }
}
