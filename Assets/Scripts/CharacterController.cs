using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Walking speed
    [SerializeField]
    private float speed;

    //Direction last moved in 
    [SerializeField]
    private Vector3 lastMoveDir;

    //List of weapons
    private GameObject[] weaponsList;

    // Start is called before the first frame update
    void Start()
    {
        lastMoveDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleDash();


        //TODO: look at cursor
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Input.mousePosition.y - transform.position.y, Input.mousePosition.x - transform.position.x) * Mathf.Rad2Deg - 90);
        //transform.LookAt(Camera.main.WorldToScreenPoint(Input.mousePosition));

        //Shoot
        if (Input.GetMouseButtonDown(0))
        {
            //spawn bullet with direction and angle
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Scroll up through weapons list
            Debug.Log("scroll up");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //scroll down through weapons list
            Debug.Log("scroll down");
        }
    }

    private void HandleMovement()
    {
        //Basic movement
        float moveX = 0f;
        float moveY = 0;
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        lastMoveDir = new Vector3(moveX, 0, moveY).normalized;
        transform.position += (lastMoveDir * speed * Time.deltaTime);
        
    }

    private void HandleDash()
    {
        if(Input.GetMouseButtonDown(1))
        {
            float dashDist = 50f;
            transform.position += lastMoveDir * dashDist;
        }
    }
}
