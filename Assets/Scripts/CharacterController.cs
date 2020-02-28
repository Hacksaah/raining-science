using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Player rigidbody
    private Rigidbody rb;

    //Walking speed
    [SerializeField]
    private float movementSpeed;

    //Direction last moved in 
    private Vector3 lastMoveDir;

    //Dash Variables
    public float dashSpeed;
    public float maxDashTime;
    private float dashTime;
    private bool dashing;

    //Weapon list
    [SerializeField]
    private Weapon[] weaponsList;

    //Current weapon in hand
    [SerializeField]
    private Weapon currentWeapon;


    // Start is called before the first frame update
    void Start()
    {
        //Set base variables
        lastMoveDir = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        dashTime = maxDashTime;
        currentWeapon = weaponsList[0];
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        HandleDash();

        FaceMouse();

        HandleGun();
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

        //If not dashing, update direction
        if(!dashing)
        {
            lastMoveDir = new Vector3(moveX, 0, moveY).normalized;
        }

        //Execute movement
        transform.position += (lastMoveDir * movementSpeed * Time.deltaTime);

    }

    private void HandleDash()
    {
        //Dash if RMB clicked and dash not in progress
        if(Input.GetMouseButtonDown(1) && !dashing)
        {
            dashTime = 0;
            dashing = true;
        }
        //Move the player for the dash
        if (dashTime < maxDashTime)
        {
            rb.velocity = lastMoveDir * dashSpeed;
            dashTime += Time.deltaTime;
        }
        //If dash just completed
        else if (dashing)
        {
            //Take away the velocity from the dash
            rb.velocity = Vector3.zero;
            dashing = false;
        }
    }

    private void FaceMouse()
    {
        //Gets player and mouse position from world
        Vector2 playerPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Computes angle
        float angle = Mathf.Atan2(playerPos.y - mousePos.y, mousePos.x - playerPos.x) * Mathf.Rad2Deg;
        //Rotates player to look at mouse
        transform.rotation = Quaternion.Euler(new Vector3(0f, angle + 90, 0f));
    }

    private void HandleGun()
    {
        //Shoot
        if (Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Shoot();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.ReloadWeapon();
        }

        //Scroll up through weapons list
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (System.Array.IndexOf(weaponsList, currentWeapon) - 1 < 0)
            {
                currentWeapon = weaponsList[weaponsList.Length - 1];
            }
            else
            {
                currentWeapon = weaponsList[System.Array.IndexOf(weaponsList, currentWeapon) - 1];
            }
        }
        //Scroll down through weapons list
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (System.Array.IndexOf(weaponsList, currentWeapon) >= weaponsList.Length - 1)
            {
                currentWeapon = weaponsList[0];
            }
            else
            {
                currentWeapon = weaponsList[System.Array.IndexOf(weaponsList, currentWeapon) + 1];
            }
        }
    }
}
