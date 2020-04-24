using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun_projectile : WeaponProjectile
{
    public LayerMask DynamicLayer;
    public LineRenderer lineRend;
    //private int damage;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    public void FireProjectile(Vector3 start, Vector3 target)
    {
        lineRend.SetPosition(0, start);
        Vector3 dir = Vector3.Normalize(target - start);
        int angleOffset = 0;
        if(dir.x < 0)
            angleOffset = 180;
        else if(dir.z < 0)
            angleOffset = 360;

        float bloom = Random.Range(-shootBloom, shootBloom);
        float angle = (Mathf.Atan(dir.z / dir.x) * Mathf.Rad2Deg + bloom + angleOffset) * Mathf.Deg2Rad;
        dir.x = Mathf.Cos(angle);
        dir.z = Mathf.Sin(angle);

        Ray ray = new Ray(start, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 75, DynamicLayer))
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                lineRend.SetPosition(1, hit.point);
                EnemyActor enemy = hit.collider.gameObject.GetComponent<EnemyActor>();                
                projectileBehavior.Deal_Damage(gameObject, enemy, damage, dir, damage_Type);                
            }
        }
        else
        {
            lineRend.SetPosition(1, (ray.origin + dir * 75));
        }
        lineRend.startColor = lightGun_projectileColor.Instance.GetColor();
        lineRend.endColor = lightGun_projectileColor.Instance.GetColor();
        StartCoroutine(FadeLine());
    }

    IEnumerator FadeLine()
    {
        float timerMax = 1f;
        float timer = timerMax;
        while(timer > 0)
        {
            timer -= Time.deltaTime;

            Color startColor = lineRend.startColor;
            Color endColor = lineRend.endColor;
            startColor.a = 0.15f * timer / timerMax;
            endColor.a = timer / timerMax;
            lineRend.startColor = startColor;
            lineRend.endColor = endColor;

            yield return null;
        }
        gameObject.SetActive(false);
    }
}
