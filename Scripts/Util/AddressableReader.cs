using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class AddressableReader
{
    AsyncOperationHandle<GameObject> operationHandle;
    public GameObject ReadGameObject(string key)
    {
        //Key����Addressable�ɓo�^����Ă���Q�[���I�u�W�F�N�g

        // Json������ǂݍ��񂾎��̕s��������B
        key = key.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");//���s�폜

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
