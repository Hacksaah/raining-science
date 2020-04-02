using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun_projectile : MonoBehaviour
{
    private LineRenderer lineRend;
    //private int damage;

    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    public void FireProjectile(Vector3 start, int damage, Vector3 target)
    {
        lineRend.SetPosition(0, start);
        Vector3 dir = (target - start).normalized;
        Ray ray = new Ray(start, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 75))
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyActor>().TakeDamage(damage, dir);
            }
            
            lineRend.SetPosition(1, hit.point);
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
