using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentButton : MonoBehaviour
{
    [SerializeField]
    private Attachment attachment;

    [SerializeField]
    private bool available;

    public Attachment Attachment { get { return attachment; } set { attachment = value; } }
    public bool Available { get { return available; } set { available = value; } }

}
