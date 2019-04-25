using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed = -1.5f;
    [SerializeField] private bool stopScrolling;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopScrolling)
            rb.velocity = Vector2.zero;
        else
            rb.velocity = new Vector2(speed, 0);
    }
}
