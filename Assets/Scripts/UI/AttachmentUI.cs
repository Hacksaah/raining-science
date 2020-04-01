using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentUI : MonoBehaviour
{
    public static AttachmentUI Instance;

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
    }
}
