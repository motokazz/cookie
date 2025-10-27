using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class AddressableReader
{
    AsyncOperationHandle<GameObject> operationHandle;
    public GameObject ReadGameObject(string key)
    {
        //KeyからAddressableに登録されているゲームオブジェクト

        // Json等から読み込んだ時の不具合を解消。
        key = key.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");//改行削除

        operationHandle = Addressables.LoadAssetAsync<GameObject>(key);
        if (operationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            return operationHandle.Result;
        }
        else
        {
            return null;
        }


}
}
