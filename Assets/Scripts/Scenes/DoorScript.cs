using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private new Animation animation;
    private bool isOpened = false;
    private bool touchingDoor = false;
    
    void Start()
    {
        animation = GetComponentInParent<Animation>();
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        print("collision");
        if (!isOpened && !touchingDoor)
        {
            animation.Play("open");
            isOpened = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("Triggered");
    }
}
