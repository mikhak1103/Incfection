using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotScript : MonoBehaviour
{
    private Transform weaponSlot;
    private Transform weaponSlot2;
    private Transform weaponSlot3;

    // Start is called before the first frame update
    void Start()
    {
        weaponSlot = transform.GetChild(0);
        weaponSlot2 = transform.GetChild(1);
        weaponSlot3 = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        weaponSlot.rotation = Quaternion.identity;
        weaponSlot2.rotation = Quaternion.identity;
        weaponSlot3.rotation = Quaternion.identity;
        transform.rotation = Quaternion.identity;
    }
}
