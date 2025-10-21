﻿using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public UpgradeUIManager upgradeUIManager;
    public EnemyManager enemyManager;

    [HideInInspector] public SaveData data;     // json変換するデータのクラス
    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名

    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            Save(data);
        }

        // ファイルを読み込んでdataに格納
        data = Load(filepath);
        
    }

    private void Start()
    {
        PutData();
        upgradeManager.InitUpgradeDataList();
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(SaveData data)
    {
        GetData();
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す

    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }

    // ゲーム中のデータを取得
    void GetData()
    {
        // メイン
        data.cookies = CookieManager.Instance.cookies;
        data.cookiesPerClick = CookieManager.Instance.cookiesPerClick;
        data.cookiesPerSecond = CookieManager.Instance.cookiesPerSecond;

        // Upgrade関連
        data.upgradeLevels = upgradeManager.LevelsToArray();

        // Enemy関連
        data.waveCount = enemyManager.waveCount;
        data.enemyCurrentHP = enemyManager.currentEnemy.currentHP;
    }

    // ゲームにロードデータを反映
    void PutData()
    {
        // メイン
        CookieManager.Instance.cookies = data.cookies;
        CookieManager.Instance.cookiesPerClick = data.cookiesPerClick;
        CookieManager.Instance.cookiesPerSecond = data.cookiesPerSecond;

        // Upgrade関連
        upgradeManager.ArrayToLevels(data.upgradeLevels);

        // Enemy関連
        enemyManager.waveCount = data.waveCount;
        enemyManager.currentEnemy.currentHP = data.enemyCurrentHP;
    }

    // データを初期化
    public void ResetData()
    {
        // メイン
        data.cookies = 0;
        data.cookiesPerClick = 1f;
        data.cookiesPerSecond = 0f;

        // Upgrade関連
        data.upgradeLevels = new int[upgradeManager.upgradeDataList.upgrades.Count];
        
        // 初期化したデータを保存
        PutData();
        Save(data);

        // 各マネージャリセット
        enemyManager.ResetEnemy();
        upgradeManager.InitUpgradeDataList();
        upgradeUIManager.RessetButtons();
    }
}
