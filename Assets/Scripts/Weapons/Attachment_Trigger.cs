using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment_Trigger : Interactable
{
    public GameObject ParentGameObject;
    public Attachment attachment;

    public int attachmentID = 0;
    private int interactId;

    private void Awake()
    {
        switch (attachmentID)
        {
            case 0:
                attachment = new IncreaseFireRate_Attachment();
                break;
            case 1:
                attachment = new TripleShotAttachment();
                break;
        }
        //ToDo the loot system should tell this attachment what to be
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // notify interact manager
            interactId = InteractManager.Instance.AddItemToQueue(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InteractManager.Instance.RemoveItemFromQueue(interactId);
        }
    }

    public override void Interact()
    {
        AttachmentUI.Instance.ActivateUI(attachment, this);
    }

    public void UpdateAttachment(int ID)
    {
        attachmentID = ID;
        switch (attachmentID)
        {
            case 0:
                attachment = new IncreaseFireRate_Attachment();
                break;
            case 1:
                attachment = new TripleShotAttachment();
                break;
        }
    }

}
