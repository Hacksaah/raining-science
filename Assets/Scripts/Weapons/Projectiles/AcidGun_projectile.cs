using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun_projectile : WeaponProjectile
{
    int split = 0;
    public GameObject acidPoolPrefab;
    public float force;

    private void Start()
    {
        acidPoolPrefab = Instantiate(acidPoolPrefab);
        acidPoolPrefab.SetActive(false);
    }

    private void OnEnable()
    {
        timeToLive = 5f;
        split = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (split > 0)
            SplitProjectile();

        if (other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity, Damage_Type.PROJECTILE);

        if (other.gameObject.layer == 9)
        {
            acidPoolPrefab.transform.position = transform.position;
            acidPoolPrefab.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void FireProjectile(float _speed, int _damage, Vector3 _target, int _split)
    {
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

            Vector3 dir = new Vector3(Random.Range(-1, 1), 0.5f, Random.Range(-1, 1));
            projectile.rb.AddForce(dir * Random.Range(2, 6), ForceMode.Impulse);
        }
        gameObject.SetActive(false);
    }
}
