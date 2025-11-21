using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MS_Random
{
    /// <summary>
    /// Vector3Random:Min・Max、それぞれののVector3の範囲でランダムなVector3を返す
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static Vector3 Vector3Random(Vector3 min, Vector3 max)
    {
        Vector3 ret = Vector3.zero;
        ret.x = UnityEngine.Random.Range(min.x, max.x);
        ret.y = UnityEngine.Random.Range(min.y, max.y);
        ret.z = UnityEngine.Random.Range(min.z, max.z);
        return ret;
    }



    /// <summary>
    /// volume内の点をランダムに抽出
    /// </summary>
    /// <param name="spawnVolume"></param>
    /// <returns></returns>
    public static Vector3 GetRandomPositionInSpawnVolume(GameObject spawnVolume)
    {
        Vector3 rnd = Vector3.zero;
        if (spawnVolume != null)
        {
            var vol = spawnVolume.GetComponent<MeshFilter>();
            if (vol != null)
            {
                var min = vol.mesh.bounds.min;
                var max = vol.mesh.bounds.max;

                min.x *= spawnVolume.transform.localScale.x;
                min.y *= spawnVolume.transform.localScale.y;
                min.z *= spawnVolume.transform.localScale.z;
                max.x *= spawnVolume.transform.localScale.x;
                max.y *= spawnVolume.transform.localScale.y;
                max.z *= spawnVolume.transform.localScale.z;

                rnd.x = UnityEngine.Random.Range(min.x, max.x);
                rnd.y = UnityEngine.Random.Range(min.y, max.y);
                rnd.z = UnityEngine.Random.Range(min.z, max.z);

            }
        }

        rnd.x += spawnVolume.transform.position.x;
        rnd.y += spawnVolume.transform.position.y;
        rnd.z += spawnVolume.transform.position.z;

        return rnd;
    }
}
