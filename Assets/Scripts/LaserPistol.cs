using UnityEngine;

public class LaserPistol : Weapon
{
    public GameObject laserBullet;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0f;
        reloadSpeed = 0.5f;
        projectileSpeed = 1000f;
        critRate = 0.2f;

        damage = 20;
        clipSize = 6;
        ammoInClip = clipSize;
        maxAmmoCapacity = 30;
        currentAmmoCapacity = maxAmmoCapacity;

        name = "Laser Pistol";
    }

    // This is here for testing purposes and will be removed during integration
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (ammoInClip > 0)
        {
            ammoInClip--;

            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = new Quaternion(0, transform.rotation.y + 1, 0, 0);

            GameObject b = Instantiate(laserBullet, transform.position /*+ (transform.parent.transform.forward / 2)*/, Quaternion.identity/*laserBullet.transform.rotation, null*/);
            b.GetComponent<Rigidbody>().AddForce(Vector3.forward * projectileSpeed);/*velocity = transform.parent.transform.forward * projectileSpeed;*/
        }
    }
}
