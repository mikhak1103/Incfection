using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Weapon : MonoBehaviour
{
    PlayerScript ps;
    WeaponController weaponController;

    public enum FireMode { Auto, Burst, Single, Pistol, Bazooka, FlameThrower};
    public FireMode fireMode;

    public GameObject smallDestroyEffect;
    public GameObject mediumDestroyEffect;
    public GameObject flames;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float timeBetweenShots;
    public int muzzleVelocity;
    public int burstCount;
    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    public float damage;
    public GameObject bazookaBulletEffect;
    GameObject muzzleFlash;

    void Start()
    {
        weaponController = GameObject.Find("Player").GetComponent<WeaponController>();
        shotsRemainingInBurst = burstCount;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        muzzleFlash = Resources.Load("Prefabs/MuzzleFlash") as GameObject;
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if(fireMode == FireMode.Burst)
            {
                if(shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }

            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot)
                    return;
            }

            else if (fireMode == FireMode.Pistol)
            {
                if (!triggerReleasedSinceLastShot)
                    return;
            }

            else if (fireMode == FireMode.Bazooka)
            {
                if (!triggerReleasedSinceLastShot)
                    return;
            }

            if (fireMode == FireMode.Burst && weaponController.shotgunAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    AudioManager.instance.PlaySound2D("Shotgun");
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    newProjectile.damage = 10 * ps.damage;
                    GameObject muzzleFlasher = Instantiate(muzzleFlash, transform.GetChild(0).transform.position, projectileSpawn[0].rotation);
                    Destroy(muzzleFlasher, 0.05f);
                }
            }


            if (fireMode == FireMode.Single && weaponController.sniperRifleAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    AudioManager.instance.PlaySound2D("Pistol");
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = mediumDestroyEffect;
                    newProjectile.damage = 75 * ps.damage;
                    GameObject muzzleFlasher = Instantiate(muzzleFlash, transform.GetChild(0).transform.position, projectileSpawn[0].rotation);
                    Destroy(muzzleFlasher, 0.05f);
                }
            }

            if (fireMode == FireMode.Bazooka && weaponController.bazookaAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    AudioManager.instance.PlaySound2D("Bazooka");
                    nextShotTime = Time.time + timeBetweenShots;
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = mediumDestroyEffect;
                    newProjectile.damage = 200 * ps.damage;
                }
            }


            if (fireMode == FireMode.Pistol)
            {
                    nextShotTime = Time.time + timeBetweenShots;
                AudioManager.instance.PlaySound2D("Pistol");
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[0].position, projectileSpawn[0].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    newProjectile.damage = 15f * ps.damage;
                GameObject muzzleFlasher = Instantiate(muzzleFlash, transform.GetChild(0).transform.position, projectileSpawn[0].rotation);
                Destroy(muzzleFlasher, 0.05f);
            }

            if (fireMode == FireMode.Auto && weaponController.actionRifleAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    AudioManager.instance.PlaySound2D("ActionRifle");
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    GameObject muzzleFlasher = Instantiate(muzzleFlash, transform.GetChild(0).transform.position, projectileSpawn[0].rotation);
                    Destroy(muzzleFlasher, 0.05f);
                    newProjectile.damage = 8f * ps.damage;
                }
            }


            if (fireMode == FireMode.FlameThrower && weaponController.flamethrowerAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    AudioManager.instance.PlaySound2D("FlameThrower");
                    nextShotTime = Time.time + timeBetweenShots;
                    GameObject flame = Instantiate(flames, projectileSpawn[i].position, projectileSpawn[i].rotation) as GameObject;
                    flame.transform.SetParent(projectileSpawn[i]);
                    Destroy(flame, 1);
                    weaponController.flamethrowerAmmo--;
                }
            }

        }


    }


    public void OnTriggerHold()
    {
        if(!ps.paused)
        {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }
    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
