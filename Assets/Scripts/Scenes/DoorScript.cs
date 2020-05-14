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
            GameObject door = gameObject.transform.parent.gameObject;
            for(int i = 0; i<door.transform.childCount; i++)
            {
                DoorScript script = (DoorScript)door.transform.GetChild(i).GetComponent(typeof(DoorScript));
                script.isOpen(true);
            }
            if (gameObject.transform.parent.gameObject.transform.parent.gameObject.tag == "BossRoom")
            {
                RoomTheme script = (RoomTheme)gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent(typeof(RoomTheme));
                script.spawnBoss();
            }
        }
    }

    public void Lock(bool touch)
    {
        locked = touch;
    }

    public void isOpen(bool open)
    {
        isOpened = open;
    }

    public bool isLocked()
    {
        return locked;
    }
}
