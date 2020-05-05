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
        timeToLive = 3.5f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        // does nothing upon hitting an enemy
        else if (other.gameObject.tag == "Enemy")
            return;
        // if this bullet hits a dynamic object (not an enemy) or a static object...
        if(other.gameObject.layer == 11 || other.gameObject.layer == 9)
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
