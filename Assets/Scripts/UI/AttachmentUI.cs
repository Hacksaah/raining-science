using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentUI : MonoBehaviour
{
    public static AttachmentUI Instance;

    public GameEvent getCurrentWeapon_event;
    public Weapon CurrentlyHeldWeapon;

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

    public void ActivateUI(Attachment incomingAttachment)
    {
        gameObject.SetActive(true);
        getCurrentWeapon_event.Raise();
        gameObject.GetComponent<AttachmentPanel>().UpdatePanel(CurrentlyHeldWeapon, incomingAttachment);
    }
}
