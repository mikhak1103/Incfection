using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private BoxCollider2D bossWallCollider;
    public float bossDefeated;
    public Transform bossWall1;
    public Transform bossWall2;
    public List<Transform> bossWalls;
    public Transform whereToSpawnBoss;
    public bool bossHasSpawned;
    private GameObject[] bosses;
    private GameObject boss;
    private float bossHealth;
    private GameObject clone;
  

    // Start is called before the first frame update
    void Start()
    {
       // bossWall1 = transform.GetChild(0);
       // bossWall2 = transform.GetChild(1);



        bossWalls.Add(bossWall1);
        bossWalls.Add(bossWall2);

        bosses = Resources.LoadAll<GameObject>("Prefabs/Enemies");

        for (int i = 0; i < bosses.Length; i++)
        {
            if (bosses[i].name == "Boss1")
            {
                boss = bosses[i];
                break;
            }
        }

        for (int i = 0; i < bossWalls.Count; i++)
        {
            bossWalls[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

     

        if (!bossHasSpawned)
        {
            return;
        }
        if (!clone)
        {
            return;
        }
        //bossHealth = clone.GetComponent<Enemy>().health;
        Debug.Log(bossHealth);

        for (int i = 0; i < bossWalls.Count; i++)
        {
            if (bossHealth <= 0)
            {
                bossWalls[i].gameObject.SetActive(false);
            }
        }


    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !bossHasSpawned)
        {
            Debug.Log("exited");
            for (int i = 0; i < bossWalls.Count; i++)
            {
                bossWalls[i].gameObject.SetActive(true);
            }

            clone = Instantiate(boss, whereToSpawnBoss.position, Quaternion.identity) as GameObject;
            bossHasSpawned = true;
            Destroy(gameObject.GetComponent<Collider2D>());

        }
    }
}
