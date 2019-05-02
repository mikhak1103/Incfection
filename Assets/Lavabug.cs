using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavabug : MonoBehaviour
{
    public float speed = 3;
    public float length = 10;
    float timer;
    Vector3 startPos;
    SpriteRenderer sr;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Mathf.Round(Time.deltaTime);
        transform.position = new Vector3(transform.position.x, startPos.y + Mathf.PingPong(Time.time * speed, length), transform.position.z);
        if (timer % 5 == 0)
            sr.flipY = true;
        if (timer % 10 == 0)
            sr.flipY = false;

        Debug.Log(timer);
    }
}
