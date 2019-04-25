using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PlayerScript : LivingEntity, ITakeDamage
{
    public readonly int maxHealth = 110;
    float xInput;
    float shrinkTime;
    [Header("Player Stats")]
    [Range(1, 15)]
    public float speed;
    [Range(1, 100)]
    public float jumpVelocity;
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;
    public float damage = 1;

    public bool grounded;
    public bool isJumping;
    public bool jumpAgain;
    public bool running;
    public bool shrunk;
    public bool paused;
    bool panning;
    bool facingRight;
    public bool invincible;

    Rigidbody2D rb;
    SpriteRenderer sr;
    CameraController cc;
    Powerup pow;
    Collider2D pCol;
    WeaponController weaponController;
    private Weapon ws;
    private SpriteRenderer wsr;
    private Inventory inventory;
    private Weapon weaponScript;
    public Menu menu;

    public Animator anim;

    //public TextMeshProUGUI ammoCountPanel;

    public GameObject bloodEffect;
    public GameObject jumpEffect;
    public GameObject PauseMenu;
    public GameObject saveTextObject;
    public GameObject extraDamageEffect;
    public GameObject invincibilityDamageEffect;
    public Text effectText;
    public GameObject winPanel;

    public Transform jumpEffectPosition;
    public Transform[] bloodSources;
    public Transform damageEffectPosition;
    public Transform[] autoSaveZones;
    Transform activeSaveZone;

    public Text healthText;

    BoxCollider2D playerCollider;
    CircleCollider2D playerCircleCollider;
    Vector2 startPlayerColliderSize;
    Vector2 startplayerCircleColliderOffset;
    Vector2 startPlayerColliderOffset;

    Scene scene;
    public Scene currentLevel;


    public override void Start()
    {
        base.Start();

        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));

        if (PlayerPrefs.GetFloat("Health") != maxHealth)
        {
            health = PlayerPrefs.GetFloat("Health");
        }
        else
        {
            health = maxHealth;
        }

        //Variables setters
        speed = 10;
        jumpVelocity = 12;

        //UI 
        healthText.text = health.ToString();

        //Component getters
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ws = GameObject.Find("Player").GetComponent<Weapon>();
        cc = GameObject.Find("MainCamera").GetComponent<CameraController>();
        weaponController = GetComponent<WeaponController>();
        pCol = GetComponent<Collider2D>();

        pow = FindObjectOfType<Powerup>();

        //Set player colliders
        playerCollider = transform.GetComponent<BoxCollider2D>();
        startPlayerColliderSize = playerCollider.size;
        playerCircleCollider = transform.GetComponent<CircleCollider2D>();
        startplayerCircleColliderOffset = playerCircleCollider.offset;
        startPlayerColliderOffset = transform.GetComponent<BoxCollider2D>().offset;
    }

    void Update()
    {
        //Scene management

        //Input
        float xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);

        if (xInput > 0)
        {
            facingRight = true;
            panning = false;
            Rotate();
        }
        else if (xInput < 0)
        {
            facingRight = false;
            panning = false;
            Rotate();
        }

        if (Input.GetMouseButton(0))
        {
            weaponController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            weaponController.OnTriggerRelease();
        }

        if (Input.GetButtonDown("Jump") && grounded && !jumpAgain)
            Jump();

        else if (Input.GetButtonDown("Jump") && !jumpAgain && !grounded)
        {
            JumpAgain();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            running = true;
            speed = 13;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            running = false;
            speed = 9;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            cc.panning = true;
            panning = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            cc.panning = false;
            panning = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!paused)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        // Animation setters

        anim.SetFloat("speed", Mathf.Abs(xInput));
        anim.SetBool("jump", isJumping);
        anim.SetBool("jump2", jumpAgain);
        anim.SetBool("running", running);
        anim.SetBool("panning", panning);

        if (health <= 0)
        {            
            Continue();
            health = maxHealth;
        }
    }
    //Functions

    public void PlayerTakeHit(float damage, Transform target)
    {
        if(!invincible)
        health -= damage;
    }

    public void Resume()
    {
        paused = false;
    }

    void Rotate()
    {
        {
            if (!facingRight && !paused)
            {
                transform.Find("Bones").eulerAngles = new Vector3(0, 180, 0);
                transform.Find("IK").eulerAngles = new Vector3(0, 180, 0);
            }
            if (facingRight && !paused)
            {
                transform.Find("Bones").eulerAngles = new Vector3(0, 0, 0);
                transform.Find("IK").eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public override void Die()
    {       
        dead = true;
        Continue();
        GameObject ps = Instantiate(destroyEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(ps, 2);
        health = maxHealth - 10;
        healthText.text = health.ToString();       
    }

    public void WinStage()
    {
        winPanel.SetActive(true);
        //StartCoroutine("WinScreenTimer");
    }

    void Jump()
    {
        isJumping = true;
        rb.velocity = Vector2.up * jumpVelocity;
    }

    void JumpAgain()
    {
        jumpAgain = true;
        rb.velocity = Vector2.up * jumpVelocity;
        GameObject ps = Instantiate(jumpEffect, jumpEffectPosition.position, Quaternion.identity) as GameObject;
        ps.transform.position = jumpEffectPosition.position;
        Destroy(ps, 0.8f);
        anim.StopPlayback();
    }

    public void SavePosition(float x, float y, float z, int _currentLevel)
    {
        PlayerPrefs.SetFloat("PlayerPosX", x);
        PlayerPrefs.SetFloat("PlayerPosY", y);
        PlayerPrefs.SetFloat("PlayerPosZ", z);
        PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
    }

    public void NewGame()
    {
        PlayerPrefs.SetFloat("PlayerPosX", 0);
        PlayerPrefs.SetFloat("PlayerPosY", 0);
        PlayerPrefs.SetFloat("PlayerPosZ", 0);
        playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");
        SceneManager.LoadScene("Level1");
    }

    public void ResumeGame()
    {
        invincible = false;
        playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");
        health = PlayerPrefs.GetFloat("health");
        transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void Continue()
    {
        invincible = false;
        playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");
        health = PlayerPrefs.GetFloat("health");
        transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
        //SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SaveHealth()
    {
        PlayerPrefs.SetFloat("health", health);
    }

    //Triggers and collisions

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
            isJumping = false;
            jumpAgain = false;
        }
        else if (col.gameObject.tag == "Enemy")
        {
            grounded = true;
            isJumping = false;
            jumpAgain = false;
        }
        if (col.gameObject.tag == "DeathZone")
        {
            Continue();
            return;
            //invincible = false;
        }

        if (col.gameObject.tag == "AutosaveZone")
        {
            activeSaveZone = col.transform;
            SaveHealth();
            currentLevel = SceneManager.GetActiveScene();
            SavePosition(col.transform.position.x, col.transform.position.y, col.transform.position.z, SceneManager.GetActiveScene().buildIndex);
            StartCoroutine("SaveTextFader");
        }

        if (col.gameObject.GetComponent<Powerup>())
        {
            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Shrink")
            {
                shrinkTime = col.gameObject.GetComponent<Powerup>().shrinkTime;
                Transform bones = GameObject.Find("Bones").transform;
                BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
                Shrink(new Vector3(0.35f, 0.35f, 1), new Vector2(0.4847617f, 0.9532442f), new Vector2(0, -0.076231f), new Vector2(0, -0.58f), bones, collider, playerCircleCollider);
                shrunk = true;
                StartCoroutine("ShrinkTimer");
            }
            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Damage")
            {
                StartCoroutine("DamageTimer");
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Invincibility")
            {
                StartCoroutine("InvincibilityTimer");
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Health")
            {
                health += col.GetComponent<Powerup>().healthAmount;
                if (health > 100)
                    health = 100;
                healthText.text = health.ToString();
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Ammo")
            {
                //weaponController.gunAmmo += 50;
                weaponController.sniperRifleAmmo += 10;
                weaponController.shotgunAmmo += 150;
                weaponController.actionRifleAmmo += 300;
                weaponController.bazookaAmmo += 5;
            }

            Destroy(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
        }
        if (col.gameObject.tag == "Enemy")
        {
            grounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ShrinkDeathArea"))
        {
            if (!shrunk)
            {
                PlayerTakeHit(health, transform);
                healthText.text = health.ToString();
            }
        }

        if (col.gameObject.CompareTag("Fire"))
        {
            PlayerTakeHit(health, transform);
            healthText.text = health.ToString();
        }
        else
            return;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && !invincible)
        {
            ITakeDamage damageAbleObject = pCol.GetComponent<ITakeDamage>();
            if (damageAbleObject != null)
            {
                Vector2 moveDirectionLeft = Vector2.left;
                Vector2 moveDirectionRight = Vector2.right;

                if (transform.position.x < col.transform.position.x)
                {
                    rb.AddForce(moveDirectionLeft.normalized * 3000f);
                }
                else if (transform.position.x > col.transform.position.x)
                {
                    rb.AddForce(moveDirectionRight.normalized * 3000f);
                }

                PlayerTakeHit(col.gameObject.GetComponent<Enemy>().damage, transform);
                healthText.text = health.ToString();
                for (int i = 0; i < bloodSources.Length; i++)
                {
                    GameObject blood = Instantiate(bloodEffect, bloodSources[i].position, bloodSources[i].rotation);
                    Destroy(blood, 0.5f);
                }
            }
        }

        if (col.gameObject.CompareTag("Enemy") && invincible)
        {
            return;
        }

        if (col.gameObject.CompareTag("BossBullet"))
        {
            PlayerTakeHit(10, transform);
            healthText.text = health.ToString();
            for (int i = 0; i < bloodSources.Length; i++)
            {
                GameObject blood = Instantiate(bloodEffect, bloodSources[i].position, bloodSources[i].rotation);
                Destroy(blood, 0.5f);
            }
        }
    }

    //IEnumerators

    public IEnumerator ShrinkTimer()
    {
        Transform bones = GameObject.Find("Bones").transform;
        BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
        yield return new WaitForSeconds(shrinkTime);
        UnShrink(new Vector3(1, 1, 1), startPlayerColliderSize, startPlayerColliderOffset, startplayerCircleColliderOffset, bones, collider, playerCircleCollider);
        shrunk = false;
    }

    private IEnumerator SaveTextFader()
    {
        //Turn My game object that is set to false(off) to True(on).
        saveTextObject.SetActive(true);

        //Turn the Game Oject back off after 1 sec.
        yield return new WaitForSeconds(2);

        //Game object will turn off
        saveTextObject.SetActive(false);
    }

    private IEnumerator DamageTimer()
    {
        GameObject damageEffect = Instantiate(extraDamageEffect, damageEffectPosition.transform.position, damageEffectPosition.transform.rotation) as GameObject;
        damageEffect.transform.SetParent(transform);
        damage *= 2;
        StartCoroutine("DamageEffectText");
        yield return new WaitForSeconds(10f);
        damage *= 0.5f;
        Destroy(damageEffect);
        effectText.text = "Extra damage lost!";
        StartCoroutine("DamageEffectGoneTimer");
    }

    private IEnumerator InvincibilityTimer()
    {
        GameObject invincibilityEffect = Instantiate(invincibilityDamageEffect, damageEffectPosition.transform.position, damageEffectPosition.transform.rotation) as GameObject;
        invincibilityEffect.transform.SetParent(transform);
        invincible = true;
        StartCoroutine("InvincibilityEffectText");
        yield return new WaitForSeconds(10f);
        invincible = false;
        Destroy(invincibilityEffect);
        StartCoroutine("InvincibilityEffectGoneTimer");
    }

    private IEnumerator DamageEffectText()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Damage Increase! BEAST MODE!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator InvincibilityEffectText()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Invincible! GODSPEED!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator DamageEffectGoneTimer()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Extra damage Lost!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator InvincibilityEffectGoneTimer()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Invincibility Lost!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }


    private IEnumerator WinScreenTimer()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Level2");
        PlayerPrefs.SetFloat("PlayerPosX", 0);
        PlayerPrefs.SetFloat("PlayerPosY", 0);
        PlayerPrefs.SetFloat("PlayerPosZ", 0);
        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
        winPanel.SetActive(false);
    }

}
