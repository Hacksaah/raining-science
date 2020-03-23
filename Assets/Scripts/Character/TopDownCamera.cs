using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private Camera MainCam;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public int mouseOffset = 300;

    private void Awake()
    {
        MainCam = GetComponent<Camera>();
    }

    // LateUpdate is called after all other updates
    void LateUpdate()
    {
        Vector3 velocity = MainCam.velocity;
        Vector3 mousePos = Input.mousePosition - MainCam.WorldToScreenPoint(transform.position) - MainCam.WorldToScreenPoint(player.transform.position);
        Vector3 posToGoTo = player.transform.position + offset + new Vector3(mousePos.x / mouseOffset, 0, mousePos.y / mouseOffset);
        transform.position = Vector3.SmoothDamp(transform.position, posToGoTo, ref velocity, smoothSpeed);
        //transform.position = new Vector3(smoothPos.x, smoothPos.y, smoothPos.z);
    }
}
