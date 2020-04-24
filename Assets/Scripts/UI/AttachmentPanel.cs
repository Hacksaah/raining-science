using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentPanel : MonoBehaviour
{
    public AttachmentButton[] attachmentButtons;

    [SerializeField]
    private GameObject AttachmentTriggerPrefab;

    public GameObject p;

    //New attachment items
    public Text IncomingAttachmentName;
    public Image IncomingAttachmentIcon;
    public Text IncomingAttachmentFlavor;

    //Spritesheet with attachments
    public Sprite[] SpriteSheet;

    //Hover display items
    public Text HoverAttachmentName;
    public Text HoverAttachmentStats;
    public Text HoverAttachmentFlavor;

    //Current gun items
    public Text GunName;
    public Image GunSprite;
    public Text GunFlavor;

    private Weapon weaponToChange;
    private Attachment newAttachment;

    [SerializeField]
    private VarBool canShootSO;

    private void Start()
    {

    }

    //Sets up relevant fields in the attachment panel
    //Sets up the buttons with attachments, text fields with names, and images with sprites
    public void UpdatePanel(Weapon currentGun, Attachment attachmentToUse)
    {
        //Reset buttons
        foreach(AttachmentButton attBut in attachmentButtons)
        {
            attBut.Attachment = null;
            attBut.GetComponent<Image>().sprite = null;
        }

        //Check if attachment is null
        if(attachmentToUse == null)
        {
            newAttachment = null;
            IncomingAttachmentName.text = "No new Attachment";
            IncomingAttachmentIcon.gameObject.SetActive(false);
            IncomingAttachmentIcon.sprite = null;
            IncomingAttachmentFlavor.text = "" ;
        }
        else
        {
            newAttachment = attachmentToUse;
            IncomingAttachmentName.text = newAttachment.Name;
            IncomingAttachmentIcon.gameObject.SetActive(true);
            IncomingAttachmentIcon.sprite = SpriteSheet[newAttachment.SpriteID];
            IncomingAttachmentFlavor.text = newAttachment.FlavorText;
        }

        canShootSO.value = false;
        weaponToChange = currentGun;

        //Set gun information
        GunName.text = currentGun.Name;
        GunSprite.sprite = currentGun.GunSprite;
        GunFlavor.text = currentGun.FlavorText;

        

        //Edit button text fields and attachments to represent the current weapons attachments
        int count = 0;
        foreach(Attachment att in weaponToChange.attachments)
        {
            attachmentButtons[count].GetComponentInChildren<Text>().text = att.Name;
            attachmentButtons[count].GetComponentInChildren<Image>().sprite = SpriteSheet[att.SpriteID];
            attachmentButtons[count].Attachment = att;
            attachmentButtons[count].Available = true;
            count++;
        }


        //Loop through the remaining buttons to edit their fields as available or not
        for (int i = count; i <= 4; i++)
        {
            if (i < weaponToChange.AttachmentSlots)
            {
                attachmentButtons[i].GetComponentInChildren<Text>().text = "Available";
                attachmentButtons[i].Available = true;
            }
            else
            {
                attachmentButtons[i].GetComponentInChildren<Text>().text = "Not Available";
                attachmentButtons[i].Available = false;
            }
        }
    }

    //When an attachment is pressed, swap the unequipped one and the clicked one
    public void ExchangeAttachments(AttachmentButton button)
    {
        int buttonIndexToMoveTo = 0;

        //Various tests to make sure attachments are good to go
        if(newAttachment == null)
        {
            return;
        }
        if(!button.Available) //If the selected button is unavailable
        {
            Debug.Log("Not an available slot");
            return;
        }
        else if(weaponToChange.AttachmentSlots <= weaponToChange.attachments.Count) //Just in case attachments become full somehow
        {
            Debug.Log("Attachments full");
            return;
        }
        else if(weaponToChange.attachments.Contains(newAttachment)) //If attachment already exists
        {
            Debug.Log("Attachment already exists");
            return;
        }
        else if(weaponToChange.attachments.First == null || button.Attachment == null & button.Available) //If the selected button is available and null 
        {
            //Add first attachment or add to an empty button
            weaponToChange.AddAttachment(newAttachment);

            //Destroy the trigger
            gameObject.GetComponent<AttachmentUI>().DestroyTrigger();
            buttonIndexToMoveTo = weaponToChange.attachments.Count - 1;

            IncomingAttachmentName.text = "No new Attachment";
            IncomingAttachmentIcon.gameObject.SetActive(false);
            IncomingAttachmentFlavor.text = "";
            newAttachment = null;
        }
        else //If there is at least one attachment
        {
            //Get the correct index to move to for the swap
            int count = 0;
            foreach (Attachment att in weaponToChange.attachments)
            {
                if(att != button.Attachment)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            buttonIndexToMoveTo = count;

            //Add the attachment
            weaponToChange.AddAttachment(newAttachment);
            //Change the trigger to hold the dropped attachment
            
            gameObject.GetComponent<AttachmentUI>().SetTriggerID(button.Attachment.AttachmentID);
            newAttachment = gameObject.GetComponent<AttachmentUI>().incomingAttachmentTrigger.attachment;

            //Remove the old attachment
            weaponToChange.RemoveAttachment(button.Attachment);
        }
        //Move attachment and update panel
        StartCoroutine(MoveIcon(buttonIndexToMoveTo, 0.5f));
        UpdatePanel(weaponToChange, newAttachment);
    }

    public void DropAttachment(AttachmentButton button)
    {
        //If button doesnt have an attachment, do nothing
        if(button.Attachment == null)
        {
            return;
        }
        
        //Make new attachment
        GameObject newAttachmentPrefab = Instantiate(AttachmentTriggerPrefab, p.transform.position, Quaternion.identity, null);

        //Change the triggers ID
        newAttachmentPrefab.GetComponentInChildren<Attachment_Trigger>().DropAttachment(p.transform.forward, button.Attachment.AttachmentID);

        //Remove attachment
        weaponToChange.RemoveAttachment(button.Attachment);

        CloseMenu();
    }

    IEnumerator MoveIcon(int buttonIndex, float totalTime)
    {
        float t = 0;
        Vector3 originalPosition = IncomingAttachmentIcon.transform.position;
        //Lerp over time
        while (t < 1)
        {
            t += Time.deltaTime / totalTime;
            IncomingAttachmentIcon.transform.position = Vector3.Lerp(originalPosition, attachmentButtons[buttonIndex].transform.position, t);
            yield return null;
        }
        IncomingAttachmentIcon.transform.position = originalPosition;
    }

    public void CloseMenu()
    {
        newAttachment = null;
        canShootSO.value = true;
        gameObject.SetActive(false);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        UpdatePanel(newWeapon, newAttachment);
    }


    public void UpdateDataPanel(AttachmentButton button)
    {
        if(button.Attachment != null)
        {
            HoverAttachmentName.text = button.Attachment.Name;
            HoverAttachmentStats.text = "";
            HoverAttachmentFlavor.text = button.Attachment.FlavorText;
        }
    }

    public void UpdateDataPanelIncoming()
    {
        if (newAttachment == null)
        {
            HoverAttachmentName.text = "No new Attachment";
            HoverAttachmentStats.text = "";
            HoverAttachmentFlavor.text = "";
        }
        else
        {
            HoverAttachmentName.text = newAttachment.Name;
            HoverAttachmentStats.text = "";
            HoverAttachmentFlavor.text = newAttachment.FlavorText;
        }
    }

    public void ClearDataPanel()
    {
        HoverAttachmentName.text = "Name";
        HoverAttachmentStats.text = "Stat Changes";
        HoverAttachmentFlavor.text = "Flavor Text";
    }
}
