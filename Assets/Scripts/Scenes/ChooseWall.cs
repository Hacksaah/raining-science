using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWall : MonoBehaviour
{
    public GameObjectSet walls;
    public PhysicMaterial material;
    
    void Awake()
    {
        if(gameObject.transform.childCount == 1)
        {
            GameObject.Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        int index = (int)UnityEngine.Random.Range(0, walls.items.Count);
        GameObject wall = GameObject.Instantiate((GameObject)walls.items.ToArray().GetValue(index));
        wall.tag = "Wall";
        wall.layer = LayerMask.NameToLayer("StaticEnvironment");
        wall.AddComponent<MeshCollider>();
        wall.GetComponent<MeshCollider>().material = material;
        wall.AddComponent<BoxCollider>();
        wall.GetComponent<BoxCollider>().material = material;
        wall.GetComponent<BoxCollider>().isTrigger = true;
        wall.GetComponent<BoxCollider>().size = new Vector3(wall.GetComponent<BoxCollider>().size.x, wall.GetComponent<BoxCollider>().size.y, 0.5f);
        wall.transform.parent = gameObject.transform;
        wall.transform.localPosition = new Vector3(0f, 0f, 0f);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wall.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
