using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public TeleportLocations teleportTo;
    MemoryFragmentScript mfs;
    
    GameObject ps;
    PortalsScript psScript;
    public bool eligibleToTeleport;
    bool insideTrigger;
    public bool startActivated;

    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("Portals");
        psScript = ps.GetComponent<PortalsScript>();
        psScript.portals.Add(gameObject);
        if (startActivated)
            gameObject.SetActive(true);
        else if (!startActivated)
            gameObject.SetActive(false);       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && insideTrigger)
        {
            eligibleToTeleport = true;              
        }

        else if (Input.GetKeyUp(KeyCode.E))
        {
            eligibleToTeleport = false;
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Press E to enter");
            {
                insideTrigger = true;
                if (eligibleToTeleport)
                {
                    foreach (GameObject portal in psScript.portals)
                    {
                        if (teleportTo.ToString() == portal.name && eligibleToTeleport)
                        {
                            eligibleToTeleport = false;
                            col.transform.position = portal.transform.position;                           
                        }
                    }
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        eligibleToTeleport = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
    }


    public enum TeleportLocations
    {
        Portal1,
        Portal2,
        Portal3,
        Portal4,
        Portal5,
        Portal6,
        Portal7,
        Portal8,
        Portal9,
        Portal10,
        Portal11,
        Portal12,
        Portal13,
        Portal14,
        Portal15,
        Portal16,
        Portal17,
        Portal18,
        Portal19,
        Portal20
    }
}
