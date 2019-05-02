using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameProjectile : MonoBehaviour
{
    int speed = 10;
    public float damage;
    Vector3 moveDirection;
    public float lifeTime = 0.1f;
    public LayerMask toHit;
    public float distance = 0.001f;
    WeaponController weaponController;
    public GameObject destroyEffect;

    private void Start()
    {
        weaponController = GameObject.Find("Player").GetComponent<WeaponController>();
        moveDirection = (transform.position - weaponController.weaponHold.transform.position);
        moveDirection.z = 0;
        Destroy(gameObject, lifeTime);
    }

    public void SetSpeed(int newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, distance, toHit);
        if (hit.collider != null)
        {
            GameObject particle = Instantiate(destroyEffect, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
            Destroy(particle, 0.1f);
            OnHitObject(hit);
        }
        transform.position = transform.position + moveDirection * Time.deltaTime * speed;
        speed = weaponController.projectileSpeed;
    }

    void OnHitObject(RaycastHit2D hit)
    {
        IDamageable damageAbleObject = hit.collider.GetComponent<IDamageable>();
        if (damageAbleObject != null)
        {
            damageAbleObject.TakeHit(damage, hit);
        }
        Destroy(gameObject);
    }
}
