using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun_projectile : WeaponProjectile
{
    int split = 0;
    public float force;

    private void OnEnable()
    {
        timeToLive = 5f;
        split = 0;
        transform.localScale = Vector3.one / 1.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (split > 0 && other.gameObject.layer != 12)
            SplitProjectile();

        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity, Damage_Type.CORROSIVE);

        if (other.gameObject.layer == 9)
        {
            GameObject acidPool = GameObjectPoolManager.RequestItemFromPool("acidPool");
            acidPool.transform.position = transform.position;
            gameObject.SetActive(false);
        }
    }

    public void FireProjectile(float _speed, int _damage, Vector3 _target, int _split)
    {
        transform.localScale = Vector3.one;
        split = _split;
        speed = _speed;
        damage = _damage;
        transform.LookAt(_target);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void SplitProjectile()
    {
        int amount = Random.Range(4, 6);
        Vector3 splitLocation = transform.position;
        splitLocation.y = splitLocation.y + 1;
        for(int i = 0; i < amount; i++)
        {
            AcidGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool("acidGun").GetComponent<AcidGun_projectile>();

            projectile.transform.position = splitLocation;

            Vector3 dir = new Vector3(Random.Range(-1, 2), 0.5f, Random.Range(-1, 2));
            projectile.rb.AddForce(dir * Random.Range(2, 7), ForceMode.Impulse);
        }
        gameObject.SetActive(false);
    }
}
