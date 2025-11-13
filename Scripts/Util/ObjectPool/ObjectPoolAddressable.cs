using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

/// <summary>
/// オブジェクトプール Addressable対応版
/// オブジェクト種類ごとに一つインスタンスして使う
/// </summary>
public class ObjectPoolAddressable : MonoBehaviour
{
    [SerializeField] public uint initPoolSize;
    [SerializeField] public string key;

    [HideInInspector] public GameObject objectPool;

    // コレクション内のプールされたオブジェクトを格納する
    private Stack<PooledObjectAddressable> stack = new Stack<PooledObjectAddressable>();
    Spawner spawner;


    private void Awake()
    {
        if (GetComponent<Spawner>() == null)
        {
            spawner = gameObject.AddComponent<Spawner>();
        }

        SetupPool();

    }

    // プールを作成する（ラグが目立たないときに呼び出す）
    private async void SetupPool()
    {

        PooledObjectAddressable instance = null;
        for (int i = 0; i < initPoolSize; i++)
        {
            //Debug.Log(key);
            await spawner.SpawnT(key,transform.position);
            GameObject go = spawner.prefabs;
            go.transform.SetParent(transform,false);
            if (go.GetComponent<PooledObjectAddressable>() == null)
            {
                go.AddComponent<PooledObjectAddressable>();
            }

            //Spawn
            instance = go.GetComponent<PooledObjectAddressable>();
            if (instance == null)
            {
                go.AddComponent<PooledObjectAddressable>();
            }


            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }

    public async Task<PooledObjectAddressable> GetPooledObject()
    {
        Debug.Log("c");
        // プールの大きさが十分でない場合は、新しい PooledObjects をインスタンス化する
        if (stack.Count == 0)
        {
            await spawner.SpawnT(key, transform.position);
            GameObject go = spawner.prefabs;

            go.transform.SetParent(transform, false);
            if (go.GetComponent<PooledObjectAddressable>() == null)
            {
                go.AddComponent<PooledObjectAddressable>();
            }

            //Spawn
            PooledObjectAddressable newInstance = go.GetComponent<PooledObjectAddressable>();
            if (newInstance == null)
            {
                go.AddComponent<PooledObjectAddressable>();
            }

            newInstance.Pool = this;
            return newInstance;
        }

        // それ以外の場合は、リストから次のものをグラブする
        PooledObjectAddressable nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObjectAddressable pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
