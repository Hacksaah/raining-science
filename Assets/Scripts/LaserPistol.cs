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

        doubleShot = false;
        attachmentSlots = 4;

        name = "Laser Pistol";

        //Testing attachments --To be removed
        DoubleShotAttachment DS1 = new DoubleShotAttachment();
        DoubleShotAttachment DS2 = new DoubleShotAttachment();
        DoubleShotAttachment DS3 = new DoubleShotAttachment();
        DS1.SetName("Double");
        DS2.SetName("Double");
        DS3.SetName("Tester");
        AddAttachment(DS1);
        AddAttachment(DS2);
        AddAttachment(DS3);
    }

    public override void Shoot()
    {
        if (doubleShot)
        {
            ShootTwo();
            return;
        }
        if (ammoInClip > 0)
        {
            ammoInClip--;

            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = new Quaternion(0, transform.rotation.y + 1, 0, 0);

            GameObject b = Instantiate(laserBullet, transform.position + (transform.parent.transform.forward / 2), laserBullet.transform.rotation, null);
            b.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward * projectileSpeed);
        }
    }
    public void ShootTwo()
    {
        if (ammoInClip > 0)
        {
            ammoInClip--;

            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = new Quaternion(0, transform.rotation.y + 1, 0, 0);

            GameObject b = Instantiate(laserBullet, transform.position + (transform.parent.transform.forward / 2), laserBullet.transform.rotation, null);
            b.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward * projectileSpeed);
        }
        if (ammoInClip > 0)
        {
            ammoInClip--;

            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = new Quaternion(0, transform.rotation.y + 1, 0, 0);

            GameObject b = Instantiate(laserBullet, transform.position + (transform.parent.transform.forward / 2), laserBullet.transform.rotation, null);
            b.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward * projectileSpeed);
        }
    }
}
