using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    string key;
    public string Key { set; get; }

    private void OnDisable()
    {
        GameObjectPoolManager.Instance.ReturnItemToPool(gameObject, Key);
    }
}
