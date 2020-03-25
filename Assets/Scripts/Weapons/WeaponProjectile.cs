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

    public void FireProjectile(float _speed, int _damage, Vector3 _target)
    {
        speed = _speed;
        damage = _damage;
        transform.LookAt(_target);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
