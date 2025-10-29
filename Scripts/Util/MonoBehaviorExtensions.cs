using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class MonoBehaviourExtensions
{

    /// <summary>
    ///  MonoBehaviour���p�������N���X�̓��e�𕶎���Ƃ��ĕԂ��܂��B
    /// </summary>
    public static string ToString2<T>(this T obj) where T : MonoBehaviour
    {
        Type t = typeof(T);

        var txt = new System.Text.StringBuilder();

        //  GameObject�����擾
        txt.Append(((MonoBehaviour)obj).name);

        // Get Public Fields
        FieldInfo[] fields = t.GetFields();
        foreach (FieldInfo f in fields)
        {
            string fName = ObjectNames.NicifyVariableName(f.Name);
            string fValue = f.GetValue(obj).ToString();
            txt.Append(string.Format(" [{0}:{1}]", fName, fValue));
        }

        return txt.ToString();
    }
}
