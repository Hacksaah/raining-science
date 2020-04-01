using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_projectile : WeaponProjectile
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity);
        gameObject.SetActive(false);
    }

    public void FireProjectile(float _speed, int _damage, Vector3 _target)
    {
        speed = _speed;
        damage = _damage;
        transform.LookAt(_target);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
