using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PoolableOptions : ScriptableObject
{
    public GameObject prefab;
    public string key;
    public int amount;
    public bool expandPool = false;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public void AddObjectToPool(GameObject obj) { pool.Enqueue(obj); }
    public GameObject GetObjectFromPool() { return pool.Dequeue(); }
}
