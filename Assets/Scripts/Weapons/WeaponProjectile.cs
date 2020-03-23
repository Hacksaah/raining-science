using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float speed;
    public int damage;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
