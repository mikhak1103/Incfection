using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backBackgrounds;
    public Transform[] foregroundBackgrounds;
    private float[] parallaxScalesBack;
    private float[] parallaxScalesFront;
    public float smoothingBackground;
    public float smoothingForeground;
    private Transform cam;
    private Vector3 previousCamPos;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
        parallaxScalesBack = new float[backBackgrounds.Length]; //how much the layer must move in correlation to the camera
        parallaxScalesFront = new float[foregroundBackgrounds.Length];
        for (int i = 0; i < backBackgrounds.Length; i++)
        {
            parallaxScalesBack[i] = backBackgrounds[i].position.z * -1;
        }
        for (int i = 0; i < foregroundBackgrounds.Length; i++)
        {
            parallaxScalesFront[i] = foregroundBackgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < backBackgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScalesBack[i];
            //float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScalesBack[i];
            float backgroundTargetPositionX = backBackgrounds[i].position.x + parallax;
            //float backgroundTargetPositionY = backBackgrounds[i].position.y - parallaxY;
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, foregroundBackgrounds[i].position.y, backBackgrounds[i].position.z);
            backBackgrounds[i].position = Vector3.Lerp(backBackgrounds[i].position, backgroundTargetPosition, smoothingBackground * Time.deltaTime);
        }

        for (int i = 0; i < foregroundBackgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScalesFront[i];
            //float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScalesFront[i];
            float foregroundTargetPositionX = foregroundBackgrounds[i].position.x + parallax;
            //float foregroundTargetPositionY = foregroundBackgrounds[i].position.y - parallaxY;
            Vector3 foregroundTargetPosition = new Vector3(foregroundTargetPositionX, foregroundBackgrounds[i].position.y, foregroundBackgrounds[i].position.z);
            foregroundBackgrounds[i].position = Vector3.Lerp(foregroundBackgrounds[i].position, foregroundTargetPosition, smoothingForeground * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
