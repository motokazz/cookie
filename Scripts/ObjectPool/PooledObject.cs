using UnityEngine;
public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;
    public ObjectPool Pool { get => pool; set => pool = value; }

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
