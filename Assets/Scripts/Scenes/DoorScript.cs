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

    private void OnCollisionEnter(Collision col)
    {
        if (!isOpened && touchingDoor && col.gameObject.tag == "Player")
        {
            animation.Play("open");
            isOpened = true;
        }
    }

    public void doorTouching(bool touch)
    {
        touchingDoor = touch;
    }
}
