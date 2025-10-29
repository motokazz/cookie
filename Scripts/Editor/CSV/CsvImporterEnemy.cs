using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;
using System.Numerics;

public class CsvImporterEnemy : EditorWindow
{
    TextAsset csvFile;
    
    string path = "Assets/Cookie/ScriptableObjects/EnemyDataList2.asset";

    [MenuItem("MS_Tools/CSVImporterEnemy")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CsvImporterEnemy));
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

    // ===========================================
    // CSVデータをスクリプタブルオブジェクトに書き込み
    // ===========================================
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
        List<string> dataList;
        dataList = new List<string>(csvText.Split("\n"));

        //０行目ヘッダ読み込んで削除
        var headers = HeaderSplitter(dataList[0]);
        dataList.RemoveAt(0);

        // Parse
        var data = new EnemyDataList();
        data.enemyList = Parser(headers, dataList.ToArray());

        // 書き込み
        Write(data);

    }

    // ===========================================
    // メンバ変数処理
    // ===========================================
    string[] HeaderSplitter(string str)
    {
        List<string> list = new List<string>();
        var headers = str.Split(",");
        foreach (string header in headers)
        {
            list.Add(header.Replace("\r\n", "").Replace("\r", "").Replace("\n", ""));
        }
        return list.ToArray();
    }

    // ===========================================
    //  パーサー
    // ===========================================

    List<EnemyData> Parser(string[] headerList, string[] dataList)
    {
        var dl = new List<EnemyData>();
        var data = new EnemyData();

        foreach (string line in dataList)
        {

            string[] each = line.Split(",");

            if (each[0] == "")
            {
                continue;
            }

            //
            data = new();

            for (int i = 0; i < each.Length; i++) {
                
                string temp = each[i].Replace("\r\n","").Replace("\r","").Replace("\n","");

                
                // タイプ別パーサー
                Type typ = data.GetType().GetField(headerList[i]).FieldType;

                if (typ == typeof(string))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data , temp);
                }
                if (typ == typeof(int))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, int.Parse(temp));
                }
                if (typ == typeof(long))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, long.Parse(temp));
                }
                if (typ == typeof(double))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, double.Parse(temp));
                }
                if (typ == typeof(float))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, float.Parse(temp));
                }
                if (typ == typeof(decimal))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, decimal.Parse(temp));
                }
                if (typ == typeof(BigInteger))
                {
                    data.GetType().GetField(headerList[i]).SetValue(data, BigInteger.Parse(temp));
                }
                
            }
            dl.Add(data);
        }
        return dl;
    }





    // ===========================================
    // スクリプタブルオブジェクト書き込み
    // ===========================================
    void Write (EnemyDataList data )
    {
        // インスタンス化したものをアセットとして保存
        var asset =AssetDatabase.LoadAssetAtPath(path, data.GetType());
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

    List<T> Parsers<T>(string[] headerList, string[] dataList)
    where T : new()
    {
        var list = new List<T>();

        // ここで headerList と dataList を使って T 型を生成するなど
        // 例: データ1行ごとに T のプロパティを設定する処理を入れられる

        return list;
    }
}
