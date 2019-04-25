using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    public Treasures treasure;
    [Range(1, 10)]
    public int minAmount = 5;
    [Range(11, 50)]
    public int maxAmount = 20;
    private int amount;

    public bool destroyAfterUse;

    public List<GameObject> treasures;
    GameObject antiBacterialShot;
    GameObject coffeeShot;
    GameObject illuminationShot;
    Weapon ws;
    PlayerScript ps;

    // Start is called before the first frame update
    void Start()
    {
        ws = GameObject.Find("EquippedWeapon").GetComponent<Weapon>();
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();

        antiBacterialShot = (GameObject)Resources.Load("Prefabs/Projectiles/Syringe_AntiBacterialShot", typeof(GameObject));
        coffeeShot = (GameObject)Resources.Load("Prefabs/Projectiles/Syringe_CoffeeShot", typeof(GameObject));
        illuminationShot = (GameObject)Resources.Load("Prefabs/Projectiles/Syringe_IlluminationShot", typeof(GameObject));

        treasures.Add(antiBacterialShot);
        treasures.Add(coffeeShot);
        treasures.Add(illuminationShot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {

            //ps.AddHealth(40);
            if (destroyAfterUse)
                Destroy(gameObject);

            /*
            Debug.Log("Entered Chest");
            for(int i = 0; i < treasures.Count; i++)
            { 
            if("Syringe_" + treasure.ToString() == treasures[i].name)
                {
                    AddTreasure(treasures[i].name);
                    
                    
                }
            }
            */
        }
    }

    public void AddTreasure(string syringeType)
    {
        amount = Random.Range(minAmount, maxAmount);
        //ws.numOfSyringes += amount;
        Debug.Log("Added " + amount + " " + syringeType + " !");
    }

    public enum Treasures
    {
        AntiBacterialShot,
        CoffeeShot,
        IlluminationShot
    }
}
