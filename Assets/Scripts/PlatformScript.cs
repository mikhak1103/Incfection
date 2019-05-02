using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [Range(1, 20)]
    public float length = 10;
    [Range(1, 10)]
    public float speed = 2;
    public bool up;
    public bool down;

    Vector3 startPos;

    public bool isEnabled;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isEnabled)
            PingPong();
    }

    public void PingPong()
    {
        //transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.z);
        if(up)
        transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * speed,  length), transform.position.z);
        else if(down)
        transform.position = new Vector3(transform.position.x, startPos.y - Mathf.PingPong(Time.time * speed, length), transform.position.z);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isEnabled)
            other.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isEnabled)
            transform.DetachChildren();
    }
 
}
