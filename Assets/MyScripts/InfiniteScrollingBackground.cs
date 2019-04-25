using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 10f;
    Vector2 offset;
    float rotate;
 

    // Start is called before the first frame update
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(scrollSpeed * Time.deltaTime,  0);
        transform.GetComponent<SpriteRenderer>().material.mainTextureOffset += offset;
    }

}
