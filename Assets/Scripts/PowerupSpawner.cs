using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour, ISpawnPowerup
{
    public enum Powerups { Shrink, Damage, Invincibility, Health};
    public Powerups powerup;
    public float healthAmount;
    public float damageAmountMultiplier;
    public float shrinkTime;
    public GameObject[] powerups;
    public bool powerupSpawned;
    public bool powerupPickedUp;
    [Range(1, 10)]
    public float spawnTimer = 5;
    Transform spawnLoc;

    void Start()
    {
        spawnTimer = shrinkTime;
        InvokeRepeating("SpawnPowerup", 0, spawnTimer);
        spawnLoc = transform.GetChild(1);
    }

    public void SpawnPowerup()
    {       switch (powerup)
            {
                case Powerups.Shrink:
                    powerups[0].GetComponent<Powerup>().shrinkTime = shrinkTime;
                    GameObject shrinkPill = Instantiate(powerups[0], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                    powerupSpawned = true;
                Destroy(shrinkPill, spawnTimer);
                break;
            case Powerups.Damage:
                powerups[0].GetComponent<Powerup>().shrinkTime = shrinkTime;
                GameObject damagePill = Instantiate(powerups[1], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                powerupSpawned = true;
                Destroy(damagePill, spawnTimer);
                break;
            case Powerups.Invincibility:
                powerups[0].GetComponent<Powerup>().shrinkTime = shrinkTime;
                GameObject invincibilityPill = Instantiate(powerups[2], spawnLoc.transform.position, Quaternion.identity) as GameObject;
                powerupSpawned = true;
                Destroy(invincibilityPill, spawnTimer);
                break;
        }        
    }    
}
