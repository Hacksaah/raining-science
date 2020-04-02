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

    public void FireProjectile(float _speed, int _damage, float shootBloom, Vector3 _target)
    {
        speed = _speed;
        damage = _damage;
        transform.LookAt(_target);
        transform.Rotate(Vector3.up, Random.Range(-shootBloom, shootBloom));
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }
}
