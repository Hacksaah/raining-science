using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringGameObj : MonoBehaviour
{
    Vector3 moveTo;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
        moveTo = startPos;
        moveTo.y = moveTo.y + 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        // this thing just moves the gameobject up and down
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
    }
}
