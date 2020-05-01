using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment_Trigger : Interactable
{
    public GameObject ParentGameObject;
    public Attachment attachment;

    public int attachmentID =-1;

    private void Awake()
    {
        if (!ParentGameObject)
            ParentGameObject = transform.parent.gameObject;

        if (attachmentID < 0)
            attachmentID = Random.Range(0, 7);

        // Determines which attachment this trigger uses
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

    private void OnDestroy()
    {
        InteractManager.Instance.RemoveItemFromQueue(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Adds this interactable to the interact manager when a player enters the trigger box
        if(other.gameObject.tag == "Player")
        {
            // notify interact manager
            InteractManager.Instance.AddItemToQueue(this);
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        //Removes this interactable from the interact manager when player leaves the trigger box
        if(other.gameObject.tag == "Player")
        {
            InteractManager.Instance.RemoveItemFromQueue(this);
        }
    }

    public override void Interact()
    {
        AttachmentUI.Instance.ActivateUI(attachment, this);
    }

    public void DropAttachment(Vector3 dir, int ID)
    {
        switch (ID)
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
        dir.y = 0.4f;
        ParentGameObject.GetComponent<Rigidbody>().AddForce(dir * 6, ForceMode.Impulse);
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
