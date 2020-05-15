using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Pooler : ScriptableObject
{
    public GameObject prefab;
    public string key;           // the gameObject's key for the dictionary
    public int amount;           // amount to spawn upon the pooling manager's Awake() call
    public int maxAmountAllowed; // if less than 0, means infinite growth
    public bool expandPool;      // allows for this pool to grow (if necessary)

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void AddObjectToPool(GameObject obj) { pool.Enqueue(obj); }

    public void ClearPool()
    {        
        pool.Clear();
    }

    public GameObject GetObjectFromPool()
    {
        if(pool.Count > 0)
        {
            return pool.Dequeue();
        }
        
        else if (expandPool && (amount < maxAmountAllowed || maxAmountAllowed < 0))
        {
            PoolableGameObject newObj = Instantiate(prefab).GetComponent<PoolableGameObject>();
            newObj.Key = key;
            amount++;
            return newObj.gameObject;
        }
        return null;
    }
}
