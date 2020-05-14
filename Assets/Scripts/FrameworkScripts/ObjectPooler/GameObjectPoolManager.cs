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

    public static Dictionary<string, Pooler> pools = new Dictionary<string, Pooler>();

    void Awake()
    {
        foreach(Pooler options in Objects)
        {
            for(int i = 0; i < options.amount; i++)
            {
                PoolableGameObject newObj = Instantiate(options.prefab).GetComponent<PoolableGameObject>();
                newObj.transform.parent = gameObject.transform;
                newObj.key = options.key;
                newObj.gameObject.SetActive(false);
                options.AddObjectToPool(newObj.gameObject);
            }
            pools.Add(options.key, options);
        }
    }

    public static void ReturnItemToPool(GameObject obj, string key)
    {
        Pooler pool;
        if (pools.TryGetValue(key, out pool))
            pool.AddObjectToPool(obj);
        else
            return;
    }

    public static GameObject RequestItemFromPool(string key)
    {
        Pooler pool;
        if (pools.TryGetValue(key, out pool))
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


