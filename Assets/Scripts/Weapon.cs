using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Weapon : MonoBehaviour
{
    PlayerScript ps;
    WeaponController weaponController;

    public enum FireMode { Auto, Burst, Single, Pistol, Bazooka};
    public FireMode fireMode;

    public GameObject smallDestroyEffect;
    public GameObject mediumDestroyEffect;

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

    void Start()
    {
        weaponController = GameObject.Find("Player").GetComponent<WeaponController>();
        shotsRemainingInBurst = burstCount;
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
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
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    newProjectile.damage = 15 * ps.damage;
                    weaponController.shotgunAmmo--;
                }
            }

            if (fireMode == FireMode.Single && weaponController.sniperRifleAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = mediumDestroyEffect;
                    newProjectile.damage = 50 * ps.damage;
                    weaponController.sniperRifleAmmo--;
                }
            }

            if (fireMode == FireMode.Bazooka && weaponController.bazookaAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = mediumDestroyEffect;
                    newProjectile.damage = 100 * ps.damage;
                    weaponController.bazookaAmmo--;
                }
            }

            if (fireMode == FireMode.Pistol && weaponController.gunAmmo > 0)
            {
                    nextShotTime = Time.time + timeBetweenShots;
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[0].position, projectileSpawn[0].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    newProjectile.damage = 8f * ps.damage;
                    //weaponController.gunAmmo--;
            }

            if (fireMode == FireMode.Auto && weaponController.actionRifleAmmo > 0)
            {
                for (int i = 0; i < projectileSpawn.Length; i++)
                {
                    nextShotTime = Time.time + timeBetweenShots;
                    Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                    newProjectile.SetSpeed(muzzleVelocity);
                    newProjectile.destroyEffect = smallDestroyEffect;
                    newProjectile.damage = 3f * ps.damage;
                    weaponController.actionRifleAmmo--;
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
