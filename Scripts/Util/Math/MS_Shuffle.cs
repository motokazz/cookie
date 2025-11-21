using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class MS_Shuffle
{

    // ===========================================
    // 順当な（バイアスの無い）シャッフル（Fisher–Yates）
    // ===========================================
    static List<int> shuffle = new List<int>();
    /// <summary>
    /// シャッフルリストの一番上の数字を返してリストから削除
    /// シャッフルリスト空になったら新規作成
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int Shuffle(int min, int max)
    {
        if (shuffle.Count == 0)
        {
            shuffle = NewShuffle(min,max);
        }
        int ret = shuffle[0];
        shuffle.RemoveAt(0);
        return ret;
    }

    /// <summary>
    /// 新しいシャッフルリストを作成
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static List<int> NewShuffle(int min,int max)
    {
        var list = Enumerable.Range(min, max - min + 1).ToList();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        //
       return list;
    }
}
