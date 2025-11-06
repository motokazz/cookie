using UnityEngine;
using TMPro;

/// <summary>
/// エネミーの表示データ
/// </summary>
/// 

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public double currentHP;

    public TMP_Text hpText;
    public TMP_Text nameText;

    private void Awake()
    {
        // Enemyコンポーネント取得
        var currentEnemy = GetComponent<Enemy>();

        if (currentEnemy == null)
        {
            currentEnemy = gameObject.AddComponent<Enemy>();
        }

        if (gameObject.tag != "Enemy")
        {
            gameObject.tag = "Enemy";
        }

    }

    // ===========================================
    // Enemyの初期化 渡したエネミーデータで上書き
    // ===========================================
    public void Initialize(int waveCount)
    {
        currentHP = data.maxHP * waveCount;

        //UI
        if (hpText != null) hpText.text = $"HP: {currentHP}";
        if (nameText != null) nameText.text = data.enemyName;
    }


}
