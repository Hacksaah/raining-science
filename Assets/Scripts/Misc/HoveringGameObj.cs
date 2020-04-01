using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// moves the gameobject up and down until disabled
public class HoveringGameObj : MonoBehaviour
{
    Vector3 moveTo;
    Vector3 startPos;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        StartCoroutine(DelayHover());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator DelayHover()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (rb.velocity.y == 0)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Hover());
    }

    
    IEnumerator Hover()
    {
        startPos = transform.position + Vector3.up * 0.35f;
        moveTo = startPos;
        moveTo.y = moveTo.y + 1.5f;
        rb.isKinematic = true;
        while (true)
        {
            transform.Rotate(Vector3.up, 0.5f);
            transform.position = Vector3.Slerp(transform.position, moveTo, 0.005f);
            if (Mathf.Abs(transform.position.y - moveTo.y) < 0.5f)
            {
                transform.position = Vector3.Slerp(transform.position, moveTo, 0.05f);
                if (startPos.y < moveTo.y)
                {
                    startPos = moveTo;
                    moveTo.y = moveTo.y - 1.5f;
                }
                else
                {
                    startPos = moveTo;
                    moveTo.y = moveTo.y + 1.5f;
                }
            }
            yield return null;
        }
    }
}
