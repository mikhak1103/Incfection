using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkDeathAreaScript : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!player.GetComponent<PlayerScript>().shrunk)
            player.GetComponent<PlayerScript>().health -= 10;
        }
    }

}
