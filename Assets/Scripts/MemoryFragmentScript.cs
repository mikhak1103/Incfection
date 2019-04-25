using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFragmentScript : MonoBehaviour
{
    PlayerScript ps;
    public GameObject portalToEnable;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePortal()
    {
        portalToEnable.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //ps.AddMemoryFragment(1);
            Debug.Log("Collected a memory fragment!");
            Destroy(gameObject);
            EnablePortal();
        }
    }

    
}
