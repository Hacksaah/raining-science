using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment_Trigger : Interactable
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
            case 1:
                attachment = new BrokenMicroscope();
                break;
            case 2:
                attachment = new HollowedRounds();
                break;
            case 3:
                attachment = new AtomDivider();
                break;
            case 4:
                attachment = new Mini3DPrinter();
                break;
            case 5:
                attachment = new PiercingRounds();
                break;
            case 6:
                attachment = new LeakingBattery();
                break;
        }
        //ToDo the loot system should tell this attachment what to be
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // notify interact manager
            InteractManager.Instance.AddItemToQueue(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            InteractManager.Instance.RemoveItemFromQueue(this);
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
                attachment = new BrokenMicroscope();
                break;
            case 2:
                attachment = new HollowedRounds();
                break;
            case 3:
                attachment = new AtomDivider();
                break;
            case 4:
                attachment = new Mini3DPrinter();
                break;
            case 5:
                attachment = new PiercingRounds();
                break;
            case 6:
                attachment = new LeakingBattery();
                break;
        }
    }

}
