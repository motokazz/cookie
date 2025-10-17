using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

        // �p�[�X�����s
        if (csvFile == null)
        {
            Debug.LogWarning(csvFile.name + " : �ǂݍ���CSV�t�@�C�����Z�b�g����Ă��܂���B");
            return;
        }

        // csv�t�@�C����string�`���ɕϊ�
        string csvText = csvFile.text;

        // ���s���ƂɃp�[�X
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
            upgradeData.baseCost = int.Parse(each[2]);
            upgradeData.currentCost = int.Parse(each[3]);
            upgradeData.cpsIncrease = int.Parse(each[4]);
            upgradeData.cpsIncreaseTotal = int.Parse(each[5]);
            upgradeData.level = int.Parse(each[6]);

            tempUpgrades.Add(upgradeData);
        }

        var data = new UpgradeDataList();
        data.upgrades = tempUpgrades;



        // �C���X�^���X���������̂��A�Z�b�g�Ƃ��ĕۑ�
        var asset = (UpgradeDataList)AssetDatabase.LoadAssetAtPath(path, typeof(UpgradeDataList));
        if (asset == null)
        {
            // �w��̃p�X�Ƀt�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬
            AssetDatabase.CreateAsset(data, path);
        }
        else
        {
            // �w��̃p�X�Ɋ��ɓ����̃t�@�C�������݂���ꍇ�͍X�V
            EditorUtility.CopySerialized(data, asset);
            AssetDatabase.SaveAssets();
        }
        AssetDatabase.Refresh();
        Debug.Log(" �f�[�^�̍쐬���������܂����B");
    }
}
