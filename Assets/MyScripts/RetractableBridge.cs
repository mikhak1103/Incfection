using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetractableBridge : MonoBehaviour
{
    public GameObject block;
    List<GameObject> blocks = new List<GameObject>();
    public int amount = 100;
    float x = 32;
    int k;
    int spawnedBlocks;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            blocks.Add(block);
        }
    }

    void ActivateBridge()
    {
        Debug.Log("hi");
        GameObject newBlock = Instantiate(block, new Vector3(transform.position.x + k, transform.position.y, 0), transform.rotation) as GameObject;
        k++;
        spawnedBlocks++;      
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        /*
        if(other.gameObject.tag == "Player")
        {
            if(spawnedBlocks <= 100)
                StartCoroutine("Wait");
        }
        */
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        Invoke("ActivateBridge", 0.1f);
    }
}
