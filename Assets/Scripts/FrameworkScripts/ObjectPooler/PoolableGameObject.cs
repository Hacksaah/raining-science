using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    [HideInInspector]
    public string key;

    private void OnDisable()
    {
        GameObjectPoolManager.ReturnItemToPool(gameObject, key);
    }
}
