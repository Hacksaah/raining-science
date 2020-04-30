using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWall : MonoBehaviour
{
    public GameObjectSet walls;
    
    void Awake()
    {
        print("Child count: "+gameObject.transform.childCount);
        if(gameObject.transform.childCount == 1)
        {
            GameObject.Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        int index = (int)UnityEngine.Random.Range(0, walls.items.Count);
        GameObject wall = GameObject.Instantiate((GameObject)walls.items.ToArray().GetValue(index));
        wall.transform.parent = gameObject.transform;
        wall.transform.localPosition = new Vector3(0f, 0f, 0f);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wall.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
