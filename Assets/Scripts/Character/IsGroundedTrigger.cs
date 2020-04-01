using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundedTrigger : MonoBehaviour
{
    public Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        // if the other gameobject is part of the Static Environment layer
        if(other.gameObject.layer == 9)
        {
            rb.isKinematic = true;
            gameObject.SetActive(false);
        }
    }
}

