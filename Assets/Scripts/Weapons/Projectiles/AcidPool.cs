using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    float duration = 3f;
    public Vector3 startScale;

    private List<EnemyActor> enemiesInPool = new List<EnemyActor>();

    private void Awake()
    {
        duration = 1f;
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        duration = 3;
        Vector3 randomSize = startScale;
        randomSize.x = startScale.x * Random.Range(0.95f, 1.1f);
        randomSize.z = startScale.z * Random.Range(0.95f, 1.1f);
        transform.localScale = randomSize; 
        StartCoroutine(ReducePoolSize());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(ApplyDoT(other.gameObject.GetComponent<EnemyActor>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            StopCoroutine(ApplyDoT(other.gameObject.GetComponent<EnemyActor>()));
        }
    }

    IEnumerator ApplyDoT(EnemyActor enemy)
    {
        float timer = 0.7f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        //Debug.Log("Applying dot");
        enemy.TakeDamage(2, Vector3.zero, Damage_Type.CORROSIVE);
        StartCoroutine(ApplyDoT(enemy));
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
