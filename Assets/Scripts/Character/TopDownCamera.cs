using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public int mouseOffset = 300;
    // LateUpdate is called after all other updates
    void LateUpdate()
    {
        Vector3 velocity = GetComponent<Camera>().velocity;
        Vector3 mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position) - Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 posToGoTo = player.transform.position + offset + new Vector3(mousePos.x / mouseOffset, 0, mousePos.y / mouseOffset);
        transform.position = Vector3.SmoothDamp(transform.position, posToGoTo, ref velocity, smoothSpeed);
        //transform.position = new Vector3(smoothPos.x, smoothPos.y, smoothPos.z);
    }
}
