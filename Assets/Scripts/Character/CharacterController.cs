using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public PlayerStats stats;

    public GameEvent playerUIReady;
    public GameEvent takeDamage;

    public GameObject isGrounded;

    //Player rigidbody
    private Rigidbody rb;

    //Direction last moved in 
    [SerializeField]
    private Vector3 lastMoveDir;

    //Dash Variables
    public float maxDashTime;
    private float dashTime;
    private bool dashing;

    //Weapon list
    [SerializeField]
    private Weapon[] weaponsList;

    //Current weapon in hand
    [SerializeField]
    private Weapon currentWeapon;    

    private RaycastHit mousePos;

    // Start is called before the first frame update
    void Start()
    {
        //Set base variables
        lastMoveDir = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        SpawnPlayer();
        playerUIReady.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mousePos);

        if (!rb.isKinematic)
            isGrounded.SetActive(true);

        HandleMovement();

        HandleDash();

        FaceMouse();

        HandleGun();

        if(Input.GetKeyDown(KeyCode.E)) //TODO: Add specifics to when the menu is opened and add the attachment picked up
        {
            //Open Attachments Menu
            //Attachment newAttachment;
            //attachmentPanel.UpdatePanel(newAttachment);
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

        //If not dashing, update direction
        if(!dashing)
        {
            lastMoveDir = new Vector3(moveX, 0, moveY).normalized;
        }

        //Execute movement
        if(rb.isKinematic)
            transform.position += (lastMoveDir * stats.MoveSpeed * Time.deltaTime);
        else
            rb.AddForce(lastMoveDir * stats.MoveSpeed * Time.deltaTime);

    }

    private void FaceMouse()
    {
        Vector3 lookAtPosition = mousePos.point;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    private void HandleDash()
    {
        //Dash if RMB clicked and dash not in progress
        if(Input.GetMouseButtonDown(1) && !dashing)
        {
            dashTime = maxDashTime;
            dashing = true;
        }
        //Move the player for the dash
        if (dashTime > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + lastMoveDir, stats.DashSpeed);
            dashTime -= Time.deltaTime;
        }
        else if(dashing)
        {
            dashTime = 0;
            dashing = false;
        }
    }

    private void HandleGun()
    {
        //Shoot
        currentWeapon.Shoot(mousePos.point, stats);

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.ReloadWeapon(stats);
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
            //TODO: Update weapon sprite to currentWeapon's sprite
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
            //TODO: Update weapon sprite to currentWeapon's sprite
        }
    }

    public void TakeDamage(int damage)
    {
        stats.CurrHP = stats.CurrHP - damage;
        takeDamage.Raise();
        if(stats.CurrHP <= 0)
        {
            // ToDo player death
        }
    }

    private void SpawnPlayer()
    {
        dashTime = maxDashTime;
        stats.CurrHP = stats.MaxHP;

        currentWeapon = weaponsList[0];
        stats.AmmoInClip = currentWeapon.AmmoInClip;
        stats.AmmoCapacity = currentWeapon.MaxAmmoCapacity;
    }
}
