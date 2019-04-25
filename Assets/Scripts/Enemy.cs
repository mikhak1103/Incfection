using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking, Patroling};
    public State currentState;

    Transform target;
    public float damage = 10;

    public float engageCombatDistance;
    Vector3 startPosition;
    public float moveDistance = 10;
    public bool leftToRight;

    float timeBetweenAttacks = 1;
    public float patrolSpeed = 2;
    public bool movingRight;
    public LayerMask layerMask;

    public Transform groundDetection;

    public float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    private GameObject enemy;
    public GameObject firePoint;
    public GameObject bullet;

    [Space(15)]
    public bool scaleUpAndDown;
    [Range(1f, 10f)]
    public float scaleSpeed = 2f;
    [Range(1, 10f)]
    public float size = 3f;

    public float nextShotTime;
    public float timeBetweenShots;

    [Header("Motions")]
    public bool rotate;
    [Range(1, 10)]
    public float rotateSpeed;
    [Range(1, 10)]
    public float radius = 4f;
    private Vector2 _centre;
    private float _angle;

    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        health = startHealth;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        _centre = transform.position;
        rotateSpeed = 2f;
        radius = 4f;
        nextShotTime = 1;
        timeBetweenShots = 0.8f;
        engageCombatDistance = 17f;
}

    private void Update()
    {
            if (Vector2.Distance(transform.position, target.position) < engageCombatDistance)
            {
                currentState = State.Attacking;
            }

            if (Vector2.Distance(transform.position, target.position) > engageCombatDistance)
            {
                currentState = State.Idle;
            }

            if (currentState == State.Patroling)
            {
                Patrol();
            }
            if (currentState == State.Attacking)
            {
                Attack();
            }
            if (currentState == State.Chasing)
            {
                Chase();
            }

            if (rotate)
            {
                Rotate();
            }
        
    }

    public void Idle()
    {

    }

    public void Patrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.5f, layerMask);
        if (groundInfo.collider == false)
        {
            if(movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        RaycastHit2D wallInfoLeft = Physics2D.Raycast(groundDetection.position, Vector2.left, 0.1f, layerMask);
        if (wallInfoLeft.collider == true)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        if (scaleUpAndDown)
        {
            ScaleUpAndDown(transform);
        }
    }

    public void Attack()
    {
        if (boss)
        {
            if (Time.time > nextShotTime)
            {
                GameObject bossBullet = Instantiate(bullet, firePoint.transform.position, Quaternion.identity);
                nextShotTime = Time.time + timeBetweenShots;
            }
            
        }

    }

    public void Rotate()
    {
        _angle += rotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * radius;
        transform.position = _centre + offset;
    }

    public void Chase()
    {
        
    }

    void ScaleUpAndDown(Transform t)
    {
        Vector3 vec = new Vector3(Mathf.Sin(Time.time * scaleSpeed) + size, Mathf.Sin(Time.time * scaleSpeed) + size, Mathf.Sin(Time.time * scaleSpeed) + size);

        transform.localScale = vec;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "AmmoBox")
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<BoxCollider2D>());
    }


}
