using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    public float speed; // this projectile's move speed
    public float shootBloom; // this projectile's inaccuracy
    public Damage_Type damage_Type;
    public int damage;

    protected float timeToLive = 5f; // lifespawn of a projectile before it returns to object pool
    protected Rigidbody rb;

    protected IProjectile projectileBehavior;

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
        timeToLive = 3f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    public void AssignData(IProjectile incBehavior, int damage, float speed, float shootBloom, Damage_Type damage_Type, bool resetProj=true)
    {        
        if(resetProj)
            incBehavior.ResetValues();

        projectileBehavior = incBehavior;
        this.damage = damage;
        this.speed = speed;
        this.shootBloom = shootBloom;
        this.damage_Type = damage_Type;        
    }

    public void FireProjectile()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9) // static environment
            gameObject.SetActive(false);
        else if (other.gameObject.tag == "Enemy")
        {
            EnemyActor enemy = other.GetComponent<EnemyActor>();
            projectileBehavior.Deal_Damage(gameObject, enemy, damage, rb.velocity, damage_Type);
        }
    }
}
