using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public PlayerStats stats;

    public GameEvent playerUIReady;
    public GameEvent takeDamage;

    
    public LayerMask GroundLayer;
    private Transform groundChecker;

    //Player rigidbody
    private Rigidbody rb;

    //Direction last moved in 
    [SerializeField]
    private Vector3 moveInput;

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

    //Stops player from shooting if false
    [SerializeField]
    private VarBool canShoot;

    private RaycastHit mousePos;

    [SerializeField]
    private AttachmentPanel aPanel;

    private void Awake()
    {
        //Set base variables
        rb = GetComponent<Rigidbody>();        
        moveInput = Vector3.zero;        
        SpawnPlayer();
        canShoot.value = true;
    }

    private void Start()
    {
        groundChecker = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * stats.MoveSpeed * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mousePos);

        CheckMovementInput();

        HandleDash();

        FaceMouse();

        HandleGun();

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            aPanel.gameObject.SetActive(true);
            Attachment nullAtt = null;
            aPanel.UpdatePanel(currentWeapon, nullAtt);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            aPanel.CloseMenu();
        }
    }

    private void CheckMovementInput()
    {
        moveInput = Vector3.zero;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");
        if (moveInput != Vector3.zero)
            moveInput = moveInput.normalized;
    }

    private void FaceMouse()
    {
        Vector3 lookAtPosition = mousePos.point;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    private void HandleDash()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // if we're grounded
            if (Physics.CheckSphere(groundChecker.position, 0.2f, GroundLayer))
            {
                Vector3 dashVelocity = Vector3.Scale(moveInput, stats.DashSpeed * new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
                rb.AddForce(dashVelocity, ForceMode.VelocityChange);
            }
            else
                Debug.Log("Not grounded");
        }       
    }

    private void HandleGun()
    {
        //Shoot
        if(canShoot.value)
        {
            currentWeapon.WeaponControls(mousePos.point, stats);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.ReloadWeapon(stats);
        }

        //Scroll up through weapons list
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (System.Array.IndexOf(weaponsList, currentWeapon) - 1 < 0)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weaponsList[weaponsList.Length - 1];
                currentWeapon.gameObject.SetActive(true);
            }
            else
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weaponsList[System.Array.IndexOf(weaponsList, currentWeapon) - 1];
                currentWeapon.gameObject.SetActive(true);
            }
            //TODO: Update weapon sprite to currentWeapon's sprite
            if(aPanel.gameObject.activeSelf)
                aPanel.ChangeWeapon(currentWeapon);
        }
        //Scroll down through weapons list
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (System.Array.IndexOf(weaponsList, currentWeapon) >= weaponsList.Length - 1)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weaponsList[0];
                currentWeapon.gameObject.SetActive(true);
            }
            else
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weaponsList[System.Array.IndexOf(weaponsList, currentWeapon) + 1];
                currentWeapon.gameObject.SetActive(true);
            }
            //TODO: Update weapon sprite to currentWeapon's sprite
            if(aPanel.gameObject.activeSelf)
                aPanel.ChangeWeapon(currentWeapon);
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
        foreach (Weapon weapon in weaponsList)
        {
            weapon.gameObject.SetActive(false);
        }
        currentWeapon.gameObject.SetActive(true);
        stats.AmmoInClip = currentWeapon.AmmoInClip;
        stats.AmmoCapacity = currentWeapon.MaxAmmoCapacity;

        playerUIReady.Raise();
    }

    public void GiveCurrentWeapon()
    {
        AttachmentUI.Instance.CurrentlyHeldWeapon = currentWeapon;
    }
}
