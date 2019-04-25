using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth;
    public float health;
    protected bool dead;
    protected GameObject destroyEffect;
    protected new SpriteRenderer renderer;
    protected Material mat;
    protected Color originalColor;
    protected float flashTime = 0.1f;
    public GameObject healthPowerup;
    public GameObject ammoCrate;
    public int itemDropChance;
    public GameObject whatToActivate;
    GameObject platform;
    PlatformScript ps;
    public bool boss;
    PlayerScript playerScript;

    public virtual void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            originalColor = renderer.color;
        }
        destroyEffect = (GameObject)Resources.Load("Prefabs/Particles/BigExplosionEffect", typeof(GameObject));
    }

    public void AddHealth(float amount)
    {
        health += amount;
    }

    public void Shrink(Vector3 vector, Vector2 size, Vector2 boxOffset, Vector2 circleOffset, Transform target, BoxCollider2D collider, CircleCollider2D circle)
    {
        target.localScale = vector;
        collider.size = size;
        collider.offset = boxOffset;
        circle.offset = circleOffset;
    }

    public void UnShrink(Vector3 vector, Vector2 size, Vector2 boxOffset, Vector2 circleOffset, Transform target, BoxCollider2D collider, CircleCollider2D circle)
    {
        target.localScale = vector;
        collider.size = size;
        collider.offset = boxOffset;
        circle.offset = circleOffset;
    }

    public virtual void TakeHit(float damage, RaycastHit2D hit)
    {
        health -= damage;

        Flash();

        if (health <= 0)
        {
            Die();
            DropItem();
            Enable();
        }

    }

    public void Flash()
    {
        renderer.color = new Color(0.3f, 0.4f, 0.6f, 0.3f);
        Invoke("ResetColor", flashTime);
    }

    public void ResetColor()
    {
        renderer.color = originalColor;
    }

    public virtual void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
        GameObject ps = Instantiate(destroyEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(ps, 2);
        if (boss)
        {
            playerScript.WinStage();
        }
    }

    public void Enable()
    {
        if (whatToActivate != null)
        {
            ps = whatToActivate.GetComponent<PlatformScript>();
            ps.isEnabled = true;
        }
    }

    public void DropItem()
    {
        int y = Random.Range(0, 100);
        {
            if (y < itemDropChance)
            {
                int x = Random.Range(0, 50);
                if (x < 50)
                {
                    GameObject pu = Instantiate(healthPowerup, transform.position, transform.rotation) as GameObject;
                }
                if (y > 50)
                {
                    GameObject pu = Instantiate(ammoCrate, transform.position, Quaternion.identity) as GameObject;
                }
            }
        }
    }
}
