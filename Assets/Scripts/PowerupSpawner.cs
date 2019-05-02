using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour, ISpawnPowerup
{
    public enum Powerups { Shrink, Damage, Invincibility, Health};
    public Powerups powerup;
    public float healthAmount;
    public float shrinkTime;
    public float damageTime;
    public float invincibilityTime;
    public GameObject[] powerups;
    public bool powerupSpawned;
    public bool powerupPickedUp;
    [Range(1, 20)]
    public float spawnTimer = 10;
    Transform spawnLoc;

    void Start()
    {
        InvokeRepeating("SpawnPowerup", 0, spawnTimer);
        spawnLoc = transform.GetChild(1);
    }

    public void SpawnPowerup()
    {
        switch (powerup)
            {
                case Powerups.Shrink:
                    powerups[0].GetComponent<Powerup>().shrinkTime = shrinkTime;
                    GameObject shrinkPill = Instantiate(powerups[0], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                spawnTimer = shrinkTime;
                    powerupSpawned = true;
                Destroy(shrinkPill, shrinkTime);
                break;
            case Powerups.Damage:
                powerups[1].GetComponent<Powerup>().damageTime = damageTime;
                GameObject damagePill = Instantiate(powerups[1], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                powerupSpawned = true;
                spawnTimer = damageTime;
                Destroy(damagePill, damageTime);
                break;
            case Powerups.Invincibility:
                powerups[2].GetComponent<Powerup>().invincibilityTime = invincibilityTime;
                GameObject invincibilityPill = Instantiate(powerups[2], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                powerupSpawned = true;
                spawnTimer = invincibilityTime;
                Destroy(invincibilityPill, invincibilityTime);
                break;
        }        
    }    
}
