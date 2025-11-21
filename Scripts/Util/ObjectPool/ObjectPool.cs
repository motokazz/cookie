using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプール
/// オブジェクト種類ごとに一つインスタンスして使う
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private uint initPoolSize;
    [SerializeField] private GameObject objectToPool;
    // コレクション内のプールされたオブジェクトを格納する
    private Stack<PooledObject> stack = new Stack<PooledObject>();

    private void Awake()
    {
        SetupPool();
    }

    // プールを作成する（ラグが目立たないときに呼び出す）
    private void SetupPool()
    {
        PooledObject instance = null;
        for (int i = 0; i < initPoolSize; i++)
        {
            var go = Instantiate(objectToPool, transform);

            if (go.GetComponent<PooledObject>() == null)
            {
                go.AddComponent<PooledObject>(); 
            }

            instance = go.GetComponent<PooledObject>();
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }

    public PooledObject GetPooledObject()
    {
        // プールの大きさが十分でない場合は、新しい PooledObjects をインスタンス化する
        if (stack.Count == 0)
        {
            var go = Instantiate(objectToPool, transform);

            if (go.GetComponent<PooledObject>() == null)
            {
                go.AddComponent<PooledObject>();
            }

            PooledObject newInstance = go.GetComponent<PooledObject>();
            newInstance.Pool = this;
            return newInstance;
        }
        // それ以外の場合は、リストから次のものをグラブする
        PooledObject nextInstance = stack.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    public void ReturnToPool(PooledObject pooledObject)
    {
        stack.Push(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }
}
