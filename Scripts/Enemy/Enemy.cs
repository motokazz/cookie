﻿using UnityEngine;
using TMPro;
/// <summary>
/// エネミーのベースデータ
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public double currentHP;

    public TMP_Text hpText;
    public TMP_Text nameText;
}
