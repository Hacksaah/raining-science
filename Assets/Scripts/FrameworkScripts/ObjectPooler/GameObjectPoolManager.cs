using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the global object pooling manager that all the poolable objects will communicate with
public class GameObjectPoolManager : MonoBehaviour
{
    public Pooler[] Objects;

    static GameObjectPoolManager instance;
    public static GameObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
                new GameObjectPoolManager();
            return instance;
        }
    }

    GameObjectPoolManager() { instance = this; }

    public Dictionary<string, Pooler> pools = new Dictionary<string, Pooler>();

    public void SpawnItemPools()
    {        
        foreach (Pooler options in Objects)
        {
            options.ClearPool();
            pools.Remove(options.key);
            for (int i = 0; i < options.amount; i++)
            {
                PoolableGameObject newObj = Instantiate(options.prefab).GetComponent<PoolableGameObject>();

                newObj.gameObject.transform.parent = gameObject.transform;
                newObj.Key = options.key;
                newObj.gameObject.SetActive(false);
                options.AddObjectToPool(newObj.gameObject);
            }

            pools.Add(options.key, options);
        }
    }

    public void ReturnItemToPool(GameObject obj, string key)
    {
        Pooler pool;
        if (pools.TryGetValue(key, out pool))
            pool.AddObjectToPool(obj);
        else
            return;
    }

    public GameObject RequestItemFromPool(string key)
    {
        if (pools.TryGetValue(key, out Pooler pool))
        {
            GameObject obj = pool.GetObjectFromPool();
            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }
}


