using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment_Trigger : MonoBehaviour
{
    public GameObject ParentGameObject;
    public Attachment attachment;

    public int attachmentID = 0;

    private void Awake()
    {
        switch (attachmentID)
        {
            case 0:
                attachment = new IncreaseFireRate_Attachment();
                break;
        }
        //ToDo the loot system should tell this attachment what to be
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // ToDo
            // Give the player this attachment...
        }
    }
}
