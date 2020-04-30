using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWall : MonoBehaviour
{
    public GameObject wall;
    void Awake()
    {
        MeshRenderer[] wallMeshes = wall.GetComponentsInChildren<MeshRenderer>();
        MeshCollider[] wallColliders = wall.GetComponentsInChildren<MeshCollider>();
        for(int i = 0; i<wallMeshes.Length; i++)
        {
            wallMeshes[i].enabled = false;
            wallColliders[i].enabled = false;
        }
        int index = (int)UnityEngine.Random.Range(0, wallMeshes.Length);
        wallMeshes[index].enabled = true;
        wallColliders[index].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
