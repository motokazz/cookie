using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Numerics;

public class CsvImporterEditor : EditorWindow
{
    TextAsset csvFile;
    ///
    string path = "Assets/Cookie/ScriptableObjects/UpgradeDataList.asset";


    [MenuItem("MS_Tools/CSVImporter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CsvImporterEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("CSVImporter");

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV",csvFile,typeof(TextAsset),true);

        if (GUILayout.Button("data create"))
        {
            CsvDataToScritableObject();
        }
    }

    void CsvDataToScritableObject()
    {

        // パースを実行
        if (csvFile == null)
        {
            Debug.LogWarning(csvFile.name + " : 読み込むCSVファイルがセットされていません。");
            return;
        }

        // csvファイルをstring形式に変換
        string csvText = csvFile.text;

        // 改行ごとにパース
        List<string> afterParse;
        afterParse = new List<string>(csvText.Split("\n"));
        afterParse.RemoveAt(0);

        List<UpgradeData> tempUpgrades = new List<UpgradeData>();

        foreach (string line in afterParse) {
            
            string[] each = line.Split(",");

            if (each[0] == "")
            {
                continue;
            }
            Debug.Log(line);
            var upgradeData = new UpgradeData();
            upgradeData.upgradeName = each[0];
            upgradeData.description = each[1];
            upgradeData.baseCost = float.Parse(each[2]);
            upgradeData.currentCost = float.Parse(each[3]);
            upgradeData.cpsIncrease = float.Parse(each[4]);
            upgradeData.cpsIncreaseTotal = float.Parse(each[5]);
            upgradeData.level = int.Parse(each[6]);

            tempUpgrades.Add(upgradeData);
        }

        var data = new UpgradeDataList();
        data.upgrades = tempUpgrades;



        // インスタンス化したものをアセットとして保存
        var asset = (UpgradeDataList)AssetDatabase.LoadAssetAtPath(path, typeof(UpgradeDataList));
        if (asset == null)
        {
            // 指定のパスにファイルが存在しない場合は新規作成
            AssetDatabase.CreateAsset(data, path);
        }
        else
        {
            // 指定のパスに既に同名のファイルが存在する場合は更新
            EditorUtility.CopySerialized(data, asset);
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.Refresh();
        Debug.Log(" データの作成が完了しました。");
    }
}
