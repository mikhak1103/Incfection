using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float distance = 0.1f;
    public int damage = 10;
    public float projectileSpeed = 10;
    public GameObject destroyEffect;
    Vector3 moveDirection;
    public LayerMask toHit;
    Transform player;
    PlayerScript ps;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("DestroyProjectile", lifeTime);
        moveDirection = (player.transform.position - transform.position);
        moveDirection.z = 0;

        ps = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, distance, toHit);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                OnHitObject(hit);
                DestroyProjectile();
            }           
        }

        transform.position = transform.position + moveDirection * speed * Time.deltaTime;
    }

    void OnHitObject(RaycastHit2D hit)
    {
        ITakeDamage damageAbleObject = hit.collider.GetComponent<ITakeDamage>();
        if (damageAbleObject != null)
        {
            damageAbleObject.PlayerTakeHit(damage);
        }
        Destroy(gameObject);
    }

    public void SetProjectileSpeed(float s)
    {
        projectileSpeed = s;
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            DestroyProjectile();
        }
    }
}
