using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentPanel : MonoBehaviour
{
    public Image currentGunImage;

    public AttachmentButton[] attachmentButtons;

    public Text UnequippedAttachment;
    public Text Title;

    private Weapon weaponToChange;
    private Attachment newAttachment;

    private void Start()
    {
        ////Testing new attachment -- to be removed
        //TripleShotAttachment TS = new TripleShotAttachment();
        //UnequippedAttachment.text = TS.Name;
        //newAttachment = TS;
    }

    //Sets up relevant fields in the attachment panel
    //Sets up the buttons with attachments, text fields with names, and images with sprites
    public void UpdatePanel(Weapon currentGun, Attachment attachmentToUse)
    {
        weaponToChange = currentGun;
        Title.text = "Max Attachments: " + weaponToChange.AttachmentSlots;
        newAttachment = attachmentToUse;
        UnequippedAttachment.text = attachmentToUse.Name;

        //Change Gun sprite
        //insert currentGun sprite
        //currentGunImage.sprite = currentGun.GunSprite;

        //Edit button text fields and attachments to represent the current weapons attachments
        int count = 0;
        foreach(Attachment att in weaponToChange.attachments)
        {
            attachmentButtons[count].GetComponentInChildren<Text>().text = att.Name;
            attachmentButtons[count].Attachment = att;
            attachmentButtons[count].Available = true;
            count++;
        }


        //Loop through the remaining buttons to edit their fields as available or not
        for (int i = count; i <= 4; i++)
        {
            if (count < weaponToChange.AttachmentSlots)
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
        if(!button.Available) //If the selected button is unavailable
        {
            Debug.Log("Not an available slot");
            return;
        }
        else if(button.Attachment == null) //If the selected button is available and null
        {
            LinkedListNode<Attachment> lastNode = weaponToChange.attachments.Last;
            LinkedListNode<Attachment> newAttachmentNode = new LinkedListNode<Attachment>(newAttachment);

            //Add new attachment after last node
            if (lastNode == null)
            {
                weaponToChange.AddAttachment(newAttachment);
            }
            else
            {
                weaponToChange.AddAttachment(newAttachment);
            }

            //TODO - Destroy attachment in the world
            //newAttachment = null;

            CloseMenu();
        }
        else //If the selected button is available and has an attachment
        {
            LinkedListNode<Attachment> current = weaponToChange.attachments.First;
            LinkedListNode<Attachment> newAttachmentNode = new LinkedListNode<Attachment>(newAttachment);
            //Get to the attachment specified
            while (current != null && current.Value != button.Attachment)
            {
                current = current.Next;
            }

            //Add new attachment after current
            weaponToChange.attachments.AddAfter(current, newAttachmentNode);

            //Place old attachment in new slot TODO: Change attachment world object to hold new attachment -- when menu is closed, send the newAttachment to the object?
            UnequippedAttachment.text = current.Value.Name;
            newAttachment = current.Value;

            //Remove old attachment
            weaponToChange.attachments.Remove(current);
        }
        UpdatePanel(weaponToChange, newAttachment);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        weaponToChange = newWeapon;
        UpdatePanel(weaponToChange, newAttachment);
    }

    public void Update()
    {
        /*
        //TODO: Change weapon based on scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //ChangeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //ChangeWeapon();
        }
        */
    }
}
