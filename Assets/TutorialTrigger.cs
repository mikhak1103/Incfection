using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    GameObject player;
    PlayerScript ps;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        ps = player.GetComponent<PlayerScript>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            ps.tutorial = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ps.tutorial = false;
        }
    }
}
