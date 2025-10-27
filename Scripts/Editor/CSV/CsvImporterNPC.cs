using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Numerics;

public class CsvImporterNPC : EditorWindow
{
    TextAsset csvFile;
    ///
    string path = "Assets/Cookie/ScriptableObjects/NPCDataList.asset";


    [MenuItem("MS_Tools/CSVImporterNPC")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CsvImporterNPC));
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

        //０行目ヘッダ読み込んで削除
        var headers = HeaderSplitter(afterParse[0]);
        Debug.Log(headers[0][0]);


        afterParse.RemoveAt(0);

        // Parse
        var data = new NPCDataList();
        data.npcList = Parser(afterParse.ToArray()); ;

        // 書き込み
        Write(data);

    }

    List<NPCData> Parser(string[] list)
    {
        List<NPCData> newList = new List<NPCData>();

        foreach (string line in list)
        {

            string[] each = line.Split(",");

            if (each[0] == "")
            {
                continue;
            }

            var item = new NPCData();
            item.npcName = each[0];
            item.attackRange = float.Parse(each[1]);
            item.attackInterval = float.Parse(each[2]);
            item.attackMultiply = float.Parse(each[3]);

            newList.Add(item);
        }
        return newList;
    }



    string[][] HeaderSplitter(string str)
    {
        List<string[]> list = new List<string[]>();
        string[] strings = new string[2];
        var headers = str.Split(",");
        foreach (string header in headers)
        {
            strings[0] =header;
            strings[1] = "0";
            list.Add(strings);
        }
        return list.ToArray();
    }






    void TypeDitect<T>(T value)
    {
        switch (value.GetType().Name)
        {
            case "Int":

                break;
            case "Double":
                break ;
            case "Float":
                break;
            case "string":
                break;

        }
    }


    void Write(NPCDataList data )
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
}
