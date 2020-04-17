using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_projectile : WeaponProjectile
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity, Damage_Type.PROJECTILE);
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
