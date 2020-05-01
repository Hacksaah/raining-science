using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentUI : MonoBehaviour
{
    public static AttachmentUI Instance;

    public GameEvent getCurrentWeapon_event;
    public Weapon CurrentlyHeldWeapon;

    public Attachment_Trigger incomingAttachmentTrigger;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.GetComponent<AttachmentPanel>().CloseMenu();
    }

    public void ActivateUI(Attachment incomingAttachment, Attachment_Trigger att_Trigger)
    {
        gameObject.SetActive(true);
        getCurrentWeapon_event.Raise();
        incomingAttachmentTrigger = att_Trigger;
        gameObject.GetComponent<AttachmentPanel>().UpdatePanel(CurrentlyHeldWeapon, incomingAttachment);
    }

    //Destroy trigger without new gameobject
    public void DestroyTrigger()
    {
        InteractManager.Instance.RemoveItemFromQueue(incomingAttachmentTrigger);
        Destroy(incomingAttachmentTrigger.ParentGameObject);
    }

    //Reset trigger with new attachment
    public void SetTriggerID(int ID)
    {
        incomingAttachmentTrigger.UpdateAttachment(ID);
    }
}
