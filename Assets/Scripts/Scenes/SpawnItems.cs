﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObjectSet items;
    public int percentChanceToSpawn = 50;
    public int numItemsToSpawn = 1;
    public Boolean collide = false;

    private int itemsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        itemsToSpawn = numItemsToSpawn;
        spawn();
    }

    public void spawn()
    {
        while (itemsToSpawn > 0)
        {
            if (random(percentChanceToSpawn))
            {
                int index = (int)UnityEngine.Random.Range(0, items.items.Count);
                GameObject item = GameObject.Instantiate((GameObject)items.items.ToArray().GetValue(index));
                item.transform.position = gameObject.transform.position;
                item.transform.localScale = gameObject.transform.localScale;
                item.transform.localRotation = gameObject.transform.localRotation;
                item.transform.parent = gameObject.transform;
                //SpawnItems itemScript = item.GetComponent<SpawnItems>();
                if (collide)
                {
                    item.AddComponent<MeshCollider>();
                    item.layer = LayerMask.NameToLayer("StaticEnvironment");
                }
            }
            itemsToSpawn--;
        }
    }

    Boolean random(int percent)
    {
        if (UnityEngine.Random.Range(0, 101) <= percent)
            return true;
        else
            return false;
    }
}
