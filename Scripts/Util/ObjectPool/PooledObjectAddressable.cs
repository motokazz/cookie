using UnityEngine;

/// <summary>
/// オブジェクトプールしたいオブジェクトにくっつけて使う Addressable対応版
/// </summary>
public class PooledObjectAddressable : MonoBehaviour
{
    private ObjectPoolAddressable pool;
    public ObjectPoolAddressable Pool { get => pool; set => pool = value; }

    public void Release()
    {
        pool.ReturnToPool(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.collider.tag;
        if ( tag == "Player" ||  tag == "Destroyer")
        {
            Release();
        }
    }
}
