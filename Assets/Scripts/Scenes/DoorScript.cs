using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private new Animation animation;
    private bool isOpened = false;
    private bool locked = false;
    
    void Start()
    {
        animation = GetComponentInParent<Animation>();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!isOpened && locked && col.gameObject.tag == "Player")
        {
            animation.Play("open");
            isOpened = true;
        }
    }

    public void Lock(bool touch)
    {
        locked = touch;
    }

    public bool isLocked()
    {
        return locked;
    }
}
