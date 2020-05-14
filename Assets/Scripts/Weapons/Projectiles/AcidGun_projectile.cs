using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun_projectile : WeaponProjectile
{
    public int split = 0;

    private void OnEnable()
    {
        split = 0;
        timeToLive = 3f;
        transform.localScale = Vector3.one / 1.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // the first projectile shot will split upon impacting anything
        if (split > 0 && other.gameObject.layer != 12 && other.gameObject.layer != 11)
            SplitProjectile();

        // perform projectile behavior
        if (other.gameObject.tag == "Enemy")
        {
            EnemyActor enemy = other.gameObject.GetComponent<EnemyActor>();
            projectileBehavior.Deal_Damage(gameObject, enemy, damage, rb.velocity, damage_Type);
        }

        // spawns an acid pool upon touching the static environment
        if (other.gameObject.layer == 9)
        {
            GameObject acidPool = GameObjectPoolManager.RequestItemFromPool("acidPool");
            acidPool.transform.position = transform.position;
            gameObject.SetActive(false);
        }
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
            projectile.AssignData(projectileBehavior, damage, speed, 0, damage_Type, false);

            Vector3 dir = new Vector3(Random.Range(-1, 2), 0.5f, Random.Range(-1, 2));
            projectile.rb.AddForce(dir * Random.Range(2, 7), ForceMode.Impulse);
        }
        gameObject.SetActive(false);
    }

    
}
