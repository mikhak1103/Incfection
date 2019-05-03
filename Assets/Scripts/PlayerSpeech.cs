using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpeech : MonoBehaviour
{
    GameObject player;
    PlayerScript ps;
    BoxCollider2D playerCol;
    GameObject speechCanvas;
    Text speechText;
    public Transform[] triggers;
    BoxCollider2D moveCol;
    BoxCollider2D jumpCol;
    BoxCollider2D shootCol;
    BoxCollider2D sprintCol;
    BoxCollider2D scrollWheelCol;
    BoxCollider2D powerUpCol;
    BoxCollider2D interactCol;
    Dictionary<string, string> monologues = new Dictionary<string, string>();
    bool move;
    bool jump;
    bool sprint;
    bool scrollwheel;
    bool powerup;
    bool shoot;
    bool interact;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        ps = player.GetComponent<PlayerScript>();
        playerCol = player.GetComponent<BoxCollider2D>();
        transform.Find("PlayerSpeecheBubbleCanvas");
        speechText = this.gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<Text>();
        speechCanvas = GameObject.Find("PlayerSpeecheBubbleCanvas");
        speechCanvas.SetActive(false);

        monologues.Add("MoveTut", "Press A and D to move me, and S to look below...I hope you can hear me..!");
        monologues.Add("JumpTut", "Hmm, what happens if I press space... And then space again?!");
        monologues.Add("SprintTut", "If I press left shift while moving I might move quicker...");
        monologues.Add("ScrollwheelTut", "Maybe if I scroll the mousewheel I can switch weapons...Press Tab to see my weapons!");
        monologues.Add("ShootTut", "What happens if I press the left mouse button?");
        monologues.Add("PowerupTut", "That looks like something I could use, what happens if I touch it?");
        monologues.Add("InteractTut", "If I press the E button I might be able to get out of here!");

        speechText.text = monologues["JumpTut"];

        moveCol = triggers[0].GetComponent<BoxCollider2D>();
        jumpCol = triggers[1].GetComponent<BoxCollider2D>();
        sprintCol = triggers[2].GetComponent<BoxCollider2D>();
        scrollWheelCol = triggers[3].GetComponent<BoxCollider2D>();
        shootCol = triggers[4].GetComponent<BoxCollider2D>();
        powerUpCol = triggers[5].GetComponent<BoxCollider2D>();
        interactCol = triggers[6].GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCol.bounds.Intersects(moveCol.bounds) && !move)
        {
            speechText.text = monologues["MoveTut"]; speechCanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
            {
                move = true;
            }
            if (move)
                StartCoroutine("Wait");
        }
        if (playerCol.bounds.Intersects(jumpCol.bounds) && !jump)
        {
            speechText.text = monologues["JumpTut"]; speechCanvas.SetActive(true);
            if(player.GetComponent<PlayerScript>().state == PlayerScript.State.Jumping)
            {
                jump = true;
            }
            if(jump)
                StartCoroutine("Wait");
        }
        if (playerCol.bounds.Intersects(sprintCol.bounds) && !sprint)
        {
            speechText.text = monologues["SprintTut"]; speechCanvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprint = true;
            }
            if(sprint)
                StartCoroutine("Wait");
        }
        if (playerCol.bounds.Intersects(scrollWheelCol.bounds) && !scrollwheel)
        {
            speechText.text = monologues["ScrollwheelTut"]; speechCanvas.SetActive(true);
            scrollwheel = true;
            if (scrollwheel)
                StartCoroutine("HideCanvas");
        }
        if (playerCol.bounds.Intersects(shootCol.bounds) && !shoot)
        {
            speechText.text = monologues["ShootTut"]; speechCanvas.SetActive(true);
            shoot = true;
            if(shoot)
                StartCoroutine("HideCanvas");
        }
        if (playerCol.bounds.Intersects(powerUpCol.bounds) && !powerup)
        {
            speechText.text = monologues["PowerupTut"]; speechCanvas.SetActive(true);         
                powerup = true;
            if (powerup)
                StartCoroutine("HideCanvas");
        }
        if (playerCol.bounds.Intersects(interactCol.bounds) && !interact)
        {
            speechText.text = monologues["InteractTut"]; speechCanvas.SetActive(true);
            interact = true;
            if (interact)
            {
                StartCoroutine("HideCanvas");
                ps.tutorial = true;
            }
        }
    }


    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        speechCanvas.SetActive(false);

    }

    private IEnumerator HideCanvas()
    {
        yield return new WaitForSeconds(4f);
        speechCanvas.SetActive(false);
    }

}
    