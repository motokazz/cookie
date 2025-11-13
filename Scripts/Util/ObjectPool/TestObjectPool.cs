using UnityEngine;

public class TestObjectPool:MonoBehaviour
{
    [SerializeField] ObjectPool pool;
    PooledObject pooledObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pooledObject = pool.GetPooledObject();
            pooledObject.transform.position = new Vector3(0,3,0);
            pooledObject.transform.rotation = Random.rotation;
            pooledObject.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere.normalized);
        }
    }
}
