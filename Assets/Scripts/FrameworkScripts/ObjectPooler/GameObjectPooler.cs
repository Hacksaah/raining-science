using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : MonoBehaviour
{
    public Pooler[] Objects;

    public static Dictionary<string, Pooler> pools = new Dictionary<string, Pooler>();

    void Awake()
    {
        foreach(Pooler options in Objects)
        {
            for(int i = 0; i < options.amount; i++)
            {
                PoolableGameObject newObj = Instantiate(options.prefab).GetComponent<PoolableGameObject>();
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
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
}


