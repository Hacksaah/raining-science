using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = new Quaternion(0, transform.rotation.y + 1, 0, 0);

        GameObject b = Instantiate(bullet, transform.position + (transform.parent.transform.forward / 2), bullet.transform.rotation, null);
        b.GetComponent<Rigidbody>().velocity = transform.parent.transform.forward * 3;
    }
}
