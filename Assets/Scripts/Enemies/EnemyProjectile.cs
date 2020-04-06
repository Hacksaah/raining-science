using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 5;

    private Rigidbody rb;

    private float timeToLive;

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
        timeToLive = 10f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<CharacterController>().TakeDamage(damage);
        gameObject.SetActive(false);
    }

    public void FireProjectile(float _speed, int _damage, int shootBloom, Vector3 _target)
    {
        speed = _speed;
        damage = _damage;
        transform.LookAt(_target);
        if (shootBloom != 0)
            transform.Rotate(Vector3.up, shootBloom);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
