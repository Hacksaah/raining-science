using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;

    public GameEvent playerUIReady;
    public GameEvent takeDamage;

    public LayerMask GroundLayer;
    private Transform groundChecker;

    //Player movement
    private CharacterController controller;
    private bool isGrounded;
    private Vector3 moveInput;
    private float moveSpeed;
    private Vector3 velocity = Vector3.zero;

    //Character controller extension
    private float pushPower = 12f;    

    //Dash Variables
    public float maxDashTime;
    private float dashDistance;
    private bool dashing;

    //Weapon list
    [SerializeField]
    private Weapon[] weaponsList = null;

    //Current weapon in hand
    [SerializeField]
    private Weapon currentWeapon;

    //Stops player from shooting if false
    [SerializeField]
    private VarBool canShoot = null;
    private bool canOpenMenus = true;

    private RaycastHit mousePos;    

    private void Awake()
    {
        //Set base variables
        controller = GetComponent<CharacterController>();
        moveInput = Vector3.zero;
        groundChecker = transform.GetChild(0);
        canShoot.value = true;
    }

    private void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mousePos);
        
        CheckMovementInput();

        HandleMovement();

        HandleDash();

        FaceMouse();

        HandleGun();


        // opens the attachment UI whenever the player holds down TAB
        if (canOpenMenus && Input.GetKeyDown(KeyCode.Tab))
        {
            AttachmentPanel.Instance.OpenPanel(currentWeapon, null);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            AttachmentPanel.Instance.CloseMenu();
        }

        //Open/close settings panel
        if (canOpenMenus && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsMenu.Instance.ActivateMenu();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;

        //Calculate push direction from move direction
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower;
    }

    private void CheckMovementInput()
    {
        moveInput = Vector3.zero;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");
        if (moveInput != Vector3.zero)
            moveInput = moveInput.normalized;
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.2f, GroundLayer);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += -9.8f * Time.deltaTime;
        controller.Move(moveInput * moveSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void FaceMouse()
    {
        Vector3 lookAtPosition = mousePos.point;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    private void HandleDash()
    {
        float drag = 8f;
        if (Input.GetMouseButtonDown(1))
        {
            // if we're grounded
            if (isGrounded)
            {                
                velocity += Vector3.Scale(moveInput, dashDistance * new Vector3((Mathf.Log(1f/(Time.deltaTime * drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * drag + 1)) / -Time.deltaTime)));
                pushPower = 25f;
            }
        }
        velocity.x /= 1 + drag * Time.deltaTime;
        velocity.z /= 1 + drag * Time.deltaTime;
        if (velocity.x == 0 && velocity.z == 0)
            pushPower = 12f;
    }

    private void HandleGun()
    {
        //Shoot
        if (canShoot.value)
        {
            currentWeapon.WeaponControls(mousePos.point, stats);
        }

        if (Input.GetKeyDown(KeyCode.R) && !currentWeapon.isReloading())
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
            AttachmentPanel.Instance.ChangeWeapon(currentWeapon);
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
            AttachmentPanel.Instance.ChangeWeapon(currentWeapon);            
        }
    }

    public void TakeDamage(int damage)
    {
        stats.CurrHP = stats.CurrHP - damage;
        takeDamage.Raise();
        if (stats.CurrHP <= 0)
        {
            canOpenMenus = false;
            gameObject.SetActive(false);
            GameOverUI.Instance.ActivateUI();
            SettingsMenu.Instance.CloseMenu();
            AttachmentPanel.Instance.CloseMenu();
        }
    }

    private void SpawnPlayer()
    {
        dashDistance = stats.DashSpeed;
        stats.CurrHP = stats.MaxHP;

        moveSpeed = stats.MoveSpeed;

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
