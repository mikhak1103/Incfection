using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShrinkTriggerTextScript : MonoBehaviour
{
    GameObject player;
    public GameObject text;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(!player.GetComponent<PlayerScript>().shrunk)
            {
                text.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerScript>().shrunk)
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}
