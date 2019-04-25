using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum Powerups { Shrink, Damage, Invincibility, Health, Ammo};
    public Powerups powerup;

    public float healthAmount;
    public float damageAmountMultiplier;
    public float shrinkTime;
    public bool pickedUp;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>(), transform.GetComponent<BoxCollider2D>());
    }
}



