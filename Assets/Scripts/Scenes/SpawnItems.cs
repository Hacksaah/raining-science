using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObjectSet items;
    public int percentChanceToSpawn = 50;
    public int numItemsToSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        while (numItemsToSpawn > 0)
        {
            numItemsToSpawn--;
            if (random(percentChanceToSpawn))
            {
                GameObject item = GameObject.Instantiate((GameObject)items.items.ToArray().GetValue((int)UnityEngine.Random.Range(0, items.items.Count)));
                item.transform.position = gameObject.transform.position;
                //SpawnItems itemScript = item.GetComponent<SpawnItems>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    Boolean random(int percent)
    {
        if (UnityEngine.Random.Range(0, 101) <= percent)
            return true;
        else
            return false;
    }
}
