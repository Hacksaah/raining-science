using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public string poolKey;
    public float speed; // projectile's move speed
    public float shootBloom;
    public Damage_Type damage_type;
    public int damage;
    public int pierceAmount; // the number of enemies to pierce, if -1: pierces inf;
    public int splitAmount; // the number of times a projectile may split after hitting an enemy
    public int splitGenerateAmount; // the number of projectiles spawned after splitting

    protected float timeToLive = 5f; // lifespawn of a projectile before it returns to object pool
    public int currPierceAmount;
    public int currSplitAmount;
    protected Rigidbody rb;

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
        timeToLive = 5f;
        currPierceAmount = pierceAmount;
        currSplitAmount = splitAmount;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    public void FireProjectile(Vector3 target)
    {
        transform.LookAt(target);
        if (shootBloom > 0)
            transform.Rotate(Vector3.up, Random.Range(-shootBloom, shootBloom));
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) // static environment
            gameObject.SetActive(false);
        else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, rb.velocity, damage_type);
            currPierceAmount--;
            if(currSplitAmount > 0)
            {
                currSplitAmount--;
            }
            if (currPierceAmount < 1)
                gameObject.SetActive(false);

        }        
    }

    private void SplitProjectile()
    {
        WeaponProjectile projectile = GameObjectPoolManager.RequestItemFromPool(poolKey).GetComponent<WeaponProjectile>();

        projectile.transform.position = transform.position + transform.forward;
        projectile.currSplitAmount = currSplitAmount;
    }
}
