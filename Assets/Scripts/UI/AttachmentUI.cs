using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentUI : MonoBehaviour
{
    public static AttachmentUI Instance;

    public GameEvent getCurrentWeapon;
    public Weapon CurrentlyHeldWeapon;
    public GameObject p;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }

    public void ActivateUI(Attachment incomingAttachment)
    {
        gameObject.SetActive(true);
        //TODO: Change p.GetComponent<LaserPistol>() to get the weapon via GameEvent
        CurrentlyHeldWeapon = p.GetComponent<LaserPistol>();
        gameObject.GetComponent<AttachmentPanel>().UpdatePanel(CurrentlyHeldWeapon, incomingAttachment);
    }
}
