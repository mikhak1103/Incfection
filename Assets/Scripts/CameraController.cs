using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    [Header("Smoothing Properties")]
    [Range(0, 0.500f)]
    public float smoothSpeed;
    [Range(0,0.500f)]
    public float panningSmoothSpeed;
    public bool panning;

    // Start is called before the first frame update
    void Start()
    {
        panning = false;
        offset = new Vector3(-0.57f, 0f, -0.45f);
        smoothSpeed = 0.125f;
        panningSmoothSpeed = 0.025f;
    }

    private void Update()
    {
        AudioListener[] aL = FindObjectsOfType<AudioListener>();
        for (int i = 0; i < aL.Length; i++)
        {
            //Destroy if AudioListener is not on the MainCamera
            if (!aL[i].CompareTag("MainCamera"))
            {
                DestroyImmediate(aL[i]);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(panning)
        {
            Vector3 desiredPosition = player.transform.position + new Vector3(-0.57f, -2, -0.45f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, panningSmoothSpeed);
            transform.position = smoothedPosition;
        }

        if (!panning)
        {
            offset = new Vector3(-0.57f, 0f, -0.45f);
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
