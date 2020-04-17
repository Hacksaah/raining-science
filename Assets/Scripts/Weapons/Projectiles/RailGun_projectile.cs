using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun_projectile : WeaponProjectile
{
    private int enemyHitCount;
    private int size;
    private int currDamage;

    public void FireProjectile(float _speed, int _damage, int _size, Vector3 _target)
    {
        size = _size;
        if(size < 3)
        {
            speed = size * _speed / 3;
            damage = size * _damage / 3;
            transform.localScale = Vector3.one * size / 3;
        }
        else
        {
            speed = _speed;
            damage = _damage;
        }
        enemyHitCount = 0;
        currDamage = damage;
        transform.LookAt(_target);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHitCount++;
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity, Damage_Type.PROJECTILE);
            if (size > 1)
            {
                if (enemyHitCount % (size * 2) == 0)
                {
                    size--;
                    transform.localScale = Vector3.one * size / 3;
                    currDamage = damage * size / 3;
                }
            }
            else if (enemyHitCount == 16)
            {
                transform.localScale = Vector3.one;
                gameObject.SetActive(false);
            }
        }
        // if collided with static environment
        if (other.gameObject.layer == 9)
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
