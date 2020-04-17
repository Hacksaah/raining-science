using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public Damage_Type damage_type;

    protected float timeToLive;
    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (timeToLive > 0)
            timeToLive -= Time.deltaTime;
        else
            gameObject.SetActive(false);
    }    

    private void OnEnable()
    {
        timeToLive = 15f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }    
}
