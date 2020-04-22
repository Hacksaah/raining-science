using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentPanel : MonoBehaviour
{
    public AttachmentButton[] attachmentButtons;

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

        canShootSO.value = false;
        weaponToChange = currentGun;

        //Set gun information
        GunName.text = currentGun.Name;
        GunSprite.sprite = currentGun.GunSprite;
        GunFlavor.text = currentGun.FlavorText;

        newAttachment = attachmentToUse;
        IncomingAttachmentName.text = newAttachment.Name;
        IncomingAttachmentIcon.sprite = SpriteSheet[newAttachment.SpriteID];
        IncomingAttachmentFlavor.text = newAttachment.FlavorText;

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
        int buttonIndex = 0;
        if(!button.Available) //If the selected button is unavailable
        {
            Debug.Log("Not an available slot");
            return;
        }
        else if(weaponToChange.AttachmentSlots <= weaponToChange.attachments.Count)
        {
            Debug.Log("Attachments full");
            return;
        }
        else if(weaponToChange.attachments.Contains(newAttachment))
        {
            Debug.Log("Attachment already exists");
            return;
        }
        else if(weaponToChange.attachments.First == null || button.Available) //If the selected button is available and null
        {
            //Add new attachment after last node
            weaponToChange.AddAttachment(newAttachment);

            buttonIndex = weaponToChange.attachments.Count - 1;
            //TODO - Destroy attachment in the world
            //newAttachment = null;
        }
        else //If the selected button is available and has an attachment
        {
            weaponToChange.AddAttachment(newAttachment);
            weaponToChange.attachments.Remove(button.Attachment);
        }
        UpdatePanel(weaponToChange, newAttachment);
        //StartCoroutine(MoveIcon(buttonIndex, 0.5f));
    }

    IEnumerator MoveIcon(int buttonIndex, float totalTime)
    {
        float t = 0;
        Vector3 originalPosition = IncomingAttachmentIcon.transform.position;
        while (t < 1)
        {
            t += Time.deltaTime / totalTime;
            IncomingAttachmentIcon.transform.position = Vector3.Lerp(originalPosition, attachmentButtons[buttonIndex].transform.position, t);
            yield return null;
        }
    }

    public void CloseMenu()
    {
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
        HoverAttachmentName.text = newAttachment.Name;
        HoverAttachmentStats.text = "";
        HoverAttachmentFlavor.text = newAttachment.FlavorText;
    }

    public void ClearDataPanel()
    {
        HoverAttachmentName.text = "Name";
        HoverAttachmentStats.text = "Stat Changes";
        HoverAttachmentFlavor.text = "Flavor Text";
    }
}
