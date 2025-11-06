using System;
using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータ書き込み・読み込み
/// </summary>
public class DataManager : MonoBehaviour
{

    [SerializeField] string dirPath = "/";
    [SerializeField] string fileName = "Data.json";              // jsonファイル名

    string filepath;

    // ===========================================
    // 開始時にファイルチェック、読み込み
    // ===========================================
    public void Init()
    {
        // パス名取得
        filepath = Application.dataPath + dirPath + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath))
        {
            Save();
        }

    }


    // ===========================================
    // jsonとしてデータを保存
    // ===========================================

    public void Save()
    {

        string json = "";

        //CookieManager
        json += ToJsonArray(GameManager.Instance.cookieManager);

        // EnemyManager
        json += ToJsonArray(GameManager.Instance.enemyManager);

        //UpgradeDataList
        json += ToJsonArray(GameManager.Instance.upgradeManager.upgradeDataList);

        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();

    }


    string ToJsonArray<T>( T data)
    {
        string json = "";
        json += data.GetType();
        json += "<SPLITTER>";
        json += JsonUtility.ToJson(data);
        json += "\n";

        return json;
    }


    public void Load()
    {
        StreamReader rd = new StreamReader(filepath);
        while (!rd.EndOfStream)
        {
            string temp = rd.ReadLine();
            if (temp == "") { continue; }

            var splitted = temp.Split("<SPLITTER>");


            switch (Type.GetType(splitted[0]).Name)
            {
                case "EnemyManager":
                    EnemyManager enemyManager = Instantiate(GameManager.Instance.enemyManager);
                    JsonUtility.FromJsonOverwrite(splitted[1], enemyManager);
                    GameManager.Instance.enemyManager.waveCount = enemyManager.waveCount;
                    break;

                case "UpgradeDataList":
                    UpgradeDataList upgradeDataList = Instantiate(GameManager.Instance.upgradeManager.upgradeDataList);
                    JsonUtility.FromJsonOverwrite(splitted[1], upgradeDataList);
                    GameManager.Instance.upgradeManager.upgradeDataList = upgradeDataList;
                    break;

                case "CookieManager":
                    CookieManager cookieManager = Instantiate(GameManager.Instance.cookieManager);
                    JsonUtility.FromJsonOverwrite(splitted[1], cookieManager);
                    GameManager.Instance.cookieManager.cookies = cookieManager.cookies;
                    GameManager.Instance.cookieManager.cookiesPerClick = cookieManager.cookiesPerClick;
                    GameManager.Instance.cookieManager.cookiesPerSecond = cookieManager.cookiesPerSecond;
                    break;
            }
        }
        rd.Close();
    }
}
