using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    float duration = 3f;
    public Vector3 startScale;

    private void Awake()
    {
        duration = 1f;
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        duration = 3;
        transform.localScale = startScale;
        StartCoroutine(ReducePoolSize());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
            other.gameObject.GetComponent<EnemyActor>().TakeDamage(2, Vector3.zero, Damage_Type.PROJECTILE);
    }

    IEnumerator ReducePoolSize()
    {
        yield return new WaitForSeconds(duration);

        while (transform.localScale.x > 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x -= Time.deltaTime;
            newScale.z -= Time.deltaTime;
            transform.localScale = newScale;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
