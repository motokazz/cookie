using System;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public CookieManager cookieManager;
    public UpgradeManager upgradeManager;
    public UpgradeUIManager upgradeUIManager;
    public EnemyManager enemyManager;

    [HideInInspector] public SaveData data;     // json変換するデータのクラス

    string filepath;                            // jsonファイルのパス
    string fileName = "Data.json";              // jsonファイル名


    // ===========================================
    // 開始時にファイルチェック、読み込み
    // ===========================================
    public void DataInit()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            Save();
        }

        // ファイルを読み込んでdataに格納
        data = Load(filepath);
        //PutData();
        upgradeManager.InitUpgradeDataList();

    }


    // ===========================================
    // jsonとしてデータを保存
    // ===========================================




    public void Save()
    {
        GetData();
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる

        SaveTest();
    }




    // jsonファイル読み込み
    SaveData Load(string path)
    {
        LoadTest(path);
        return data;
        /*
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる
        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
        */

    }



    public void SaveTest()
    {

        string json = "";


        json += "CookieManager";
        json += "<SPLITTER>";
        json += JsonUtility.ToJson(GameManager.Instance.cookieManager);
        json += "\n";

        json += "EnemyDataList";
        json += "<SPLITTER>";
        json += JsonUtility.ToJson(GameManager.Instance.enemyManager.enemyDataList);
        json += "\n";
        
        json += "UpgradeDataList";
        json += "<SPLITTER>";
        json += JsonUtility.ToJson(GameManager.Instance.upgradeManager.upgradeDataList);
        json += "\n";


        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();

    }

    void LoadTest(string path)
    {
        StreamReader rd = new StreamReader(path);
        while (!rd.EndOfStream)
        {
            string temp = rd.ReadLine();
            if (temp == "") { continue; }

            var splitted = temp.Split("<SPLITTER>");

            var tempClass = Activator.CreateInstance(Type.GetType(splitted[0]));

            if (tempClass.GetType() == typeof(EnemyDataList))
            {
                EnemyDataList fj = new EnemyDataList();
                JsonUtility.FromJsonOverwrite(splitted[1], fj);
                GameManager.Instance.enemyManager.enemyDataList = fj;
            }

            if (tempClass.GetType() == typeof(UpgradeDataList))
            {
                UpgradeDataList fj = new UpgradeDataList();
                JsonUtility.FromJsonOverwrite(splitted[1], fj);
                GameManager.Instance.upgradeManager.upgradeDataList = fj;
            }

            if (tempClass.GetType() == typeof(CookieManager))
            {
                CookieManager fj = new CookieManager();
                JsonUtility.FromJsonOverwrite(splitted[1], fj);
                GameManager.Instance.cookieManager.cookies = fj.cookies;
                GameManager.Instance.cookieManager.cookiesPerClick = fj.cookiesPerClick;
                GameManager.Instance.cookieManager.cookiesPerSecond = fj.cookiesPerSecond;
            }

        }
        rd.Close();
    }


    // ===========================================
    // データパーサー
    // ===========================================

    // GameData -> TempData
    void GetData()
    {
        // メイン
        data.cookies = cookieManager.cookies;
        data.cookiesPerClick = cookieManager.cookiesPerClick;
        data.cookiesPerSecond = cookieManager.cookiesPerSecond;

        // Upgrade関連
        data.upgradeLevels = upgradeManager.LevelsToArray();

        // Enemy関連
        data.waveCount = enemyManager.waveCount;
        data.enemyCurrentHP = enemyManager.currentEnemy.currentHP;
    }

    // TempData -> GameData
    public void PutData()
    {
        // メイン
        cookieManager.cookies = data.cookies;
        cookieManager.cookiesPerClick = data.cookiesPerClick;
        cookieManager.cookiesPerSecond = data.cookiesPerSecond;

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
        Save();

        // 各マネージャリセット
        enemyManager.ResetEnemy();
        upgradeManager.InitUpgradeDataList();
        upgradeUIManager.RessetButtons();
    }
}
