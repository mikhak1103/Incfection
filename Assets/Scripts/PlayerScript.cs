using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class PlayerScript : LivingEntity, ITakeDamage
{
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
    float damageTime;
    float invincibilityTime;
    float timeBetweenDust = 0.3f;
    float nextDustTime;

    public bool grounded;
    public bool isJumping;
    public bool jumpAgain;
    public bool running;
    public bool shrunk;
    public bool berserk;
    public bool invincible;
    public bool paused;
    bool pausedMainMenu;
    bool panning;
    bool facingRight;
    bool portalSpawned;
    public bool tutorial;

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

    GameObject bloodEffect;
    GameObject dustEffect;
    GameObject jumpEffect;
    GameObject pauseMenu;
    GameObject saveTextObject;
    public GameObject extraDamageEffect;
    public GameObject invincibilityDamageEffect;
    public GameObject timerCanvas;
    public Text timerCanvasText;
    public Text effectText;
    public GameObject winPanel;
    GameObject portal;
    public GameObject[] portalsToDeactivate;
    public GameObject ingameMainMenu;

    public Transform jumpEffectPosition;
    public Transform[] bloodSources;
    public Transform damageEffectPosition;
    public Transform dustEffectPosition;
    public Transform dustEffectPosition2;
    Transform activeSaveZone;
    Transform activeSaveZoneScript;
    public Transform tutorialStartPos;

    public Text healthText;

    BoxCollider2D playerCollider;
    CircleCollider2D playerCircleCollider;
    Vector2 startPlayerColliderSize;
    Vector2 startplayerCircleColliderOffset;
    Vector2 startPlayerColliderOffset;

    Scene scene;
    public Scene currentLevel;
    public AudioClip bossMusic;

    public enum State {Idle, Moving, Jumping, MovingJumping, Dead}
    public State state;


    public override void Start()
    {
        base.Start();
        startHealth = 100;
        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
        PlayerPrefs.SetFloat("Health", startHealth);
        health = PlayerPrefs.GetFloat("Health");

        //Variables setters
        speed = 7;
        jumpVelocity = 14;

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
        pauseMenu = GameObject.Find("PauseMenu");
        saveTextObject = GameObject.Find("SavedText");
        saveTextObject.SetActive(false);
        portal                      = Resources.Load("Prefabs/Portal/Deathportal") as GameObject;
        bloodEffect                 = Resources.Load("Prefabs/Particles/BloodEffect") as GameObject;
        jumpEffect                  = Resources.Load("Prefabs/Particles/JumpEffect") as GameObject;
        dustEffect                  = Resources.Load("Prefabs/Particles/WalkEffect") as GameObject;

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

        //State machine
        if (xInput > 0)
        {
            state = State.Moving;
            facingRight = true;
            panning = false;
            Rotate();
        }
        else if (xInput < 0)
        {
            state = State.Moving;
            facingRight = false;
            panning = false;
            Rotate();
        }
        else if (xInput == 0)
            state = State.Idle;           

        if (health <= 0)
            state = State.Dead;

        if(state == State.Moving && Time.time > nextDustTime && grounded)
        {
            nextDustTime = Time.time + timeBetweenDust;
            GameObject de = Instantiate(dustEffect, dustEffectPosition.position, Quaternion.identity) as GameObject;
            de.transform.position = dustEffectPosition.position;
            Destroy(de, 0.3f);
            GameObject de2 = Instantiate(dustEffect, dustEffectPosition2.position, Quaternion.identity) as GameObject;
            de2.transform.position = dustEffectPosition2.position;
            Destroy(de2, 0.3f);
        }

        //End state machine

        if (Input.GetMouseButton(0))
        {
            weaponController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            weaponController.OnTriggerRelease();
        }


        //Jump controls

        if (Input.GetButtonDown("Jump") && !isJumping)
            Jump();

        else if (Input.GetButtonDown("Jump") && !jumpAgain)
        {
            JumpAgain();
        }
        if (isJumping || jumpAgain)
        {
            isJumping = true;
            grounded = false;
        }
        //end Jump controls

        if (Input.GetKeyDown(KeyCode.LeftShift) && state == State.Moving)
        {
            running = true;
            speed = 13;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
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
            pausedMainMenu = !pausedMainMenu;
        }
        if (pausedMainMenu)
        {
            ingameMainMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!pausedMainMenu)
        {
            ingameMainMenu.SetActive(false);
            Time.timeScale = 1;
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            paused = !paused;
        }
        if (paused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!paused)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        switch(state)
        {
            case State.Dead:
                anim.Play("Death");   StartCoroutine("DeathTimer"); 
                break;
        }

        Debug.Log(state);
        // Animation setters

        anim.SetFloat("speed", Mathf.Abs(xInput));
        anim.SetBool("jump", isJumping);
        anim.SetBool("jump2", jumpAgain);
        anim.SetBool("running", running);
        anim.SetBool("panning", panning);
    }
    //Functions

    public void PlayerTakeHit(float damage)
    {
        if (!invincible)
        {
            AudioManager.instance.PlaySound2D("HitEnemy");
            health -= damage;
            healthText.text = health.ToString();
        }
    }

    public void Resume()
    {
        paused = false;
        pausedMainMenu = false;
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

    public void WinStage()
    {
        winPanel.SetActive(true);
        StartCoroutine("WinScreenTimer");
    }

    void Jump()
    {
        AudioManager.instance.PlaySound2D("Land");
        //AudioManager.instance.PlaySound2D("Jump");
        isJumping = true;
        rb.velocity = Vector2.up * jumpVelocity;
        state = State.Jumping;
    }

    void JumpAgain()
    {
        AudioManager.instance.PlaySound2D("Land");
        //AudioManager.instance.PlaySound2D("Jump");
        jumpAgain = true;
        rb.velocity = Vector2.up * jumpVelocity;
        GameObject ps = Instantiate(jumpEffect, jumpEffectPosition.position, Quaternion.identity) as GameObject;
        ps.transform.position = jumpEffectPosition.position;
        Destroy(ps, 0.8f);
        state = State.Jumping;
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
        SceneManager.LoadScene("Level1");
        PlayerPrefs.SetFloat("PlayerPosX", -145);
        PlayerPrefs.SetFloat("PlayerPosY", 0);
        PlayerPrefs.SetFloat("PlayerPosZ", 0);
        playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");
        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
        health = startHealth;
        tutorial = true;
        state = State.Idle;
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
        playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");
        //health = PlayerPrefs.GetFloat("health");
        transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
        state = State.Idle;
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
        if (col.CompareTag("Ground"))
        {
            grounded = true;
            isJumping = false;
            jumpAgain = false;
        }
        if (col.CompareTag("Enemy"))
        {
            grounded = true;
            isJumping = false;
            jumpAgain = false;
        }

        if (col.gameObject.CompareTag("Fire"))
        {
            if (!invincible)
                health -= 100;
            else
                return;
        }

        if (col.gameObject.CompareTag("BacteriaWaterfallDeathZone"))
        {
            if (!invincible)
                health -= 100;
            else
                return;
        }

        if (col.gameObject.CompareTag("DeathZone"))
        {
            health -= 100;
        }

        if (col.gameObject.CompareTag("BossMusicTrigger"))
        {
            AudioManager.instance.PlayMusic(bossMusic, 1);
        }

        if (col.gameObject.tag == "AutosaveZone")
        {
            activeSaveZone = col.transform;
            SaveHealth();
            currentLevel = SceneManager.GetActiveScene();
            SavePosition(col.transform.position.x, col.transform.position.y, col.transform.position.z, SceneManager.GetActiveScene().buildIndex);
            if (col.gameObject.GetComponent<AutoSaveZone>().saved == false)
            {
                AudioManager.instance.PlaySound2D("Spawn");
                StartCoroutine("SaveTextFader");
            }
            col.gameObject.GetComponent<AutoSaveZone>().saved = true;
        }

        if (col.gameObject.GetComponent<Powerup>())
        {
            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Shrink" && !berserk && !shrunk && !invincible)
            {
                AudioManager.instance.PlaySound2D("PowerUp");
                shrinkTime = col.gameObject.GetComponent<Powerup>().shrinkTime;
                Transform bones = GameObject.Find("Bones").transform;
                BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
                Shrink(new Vector3(0.35f, 0.35f, 1), new Vector2(0.4847617f, 0.9532442f), new Vector2(0, -0.076231f), new Vector2(0, -0.58f), bones, collider, playerCircleCollider);
                StartCoroutine("ShrinkTimer");
                Destroy(col.gameObject);
            }
            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Damage" && !invincible && !berserk && !shrunk)
            {
                AudioManager.instance.PlaySound2D("PowerUp");
                damageTime = col.gameObject.GetComponent<Powerup>().damageTime;
                StartCoroutine("DamageTimer");
                Destroy(col.gameObject);
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Invincibility" && !invincible && !berserk && !shrunk)
            {
                AudioManager.instance.PlaySound2D("PowerUp");
                invincibilityTime = col.gameObject.GetComponent<Powerup>().invincibilityTime;
                StartCoroutine("InvincibilityTimer");
                Destroy(col.gameObject);
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Health")
            {
                AudioManager.instance.PlaySound2D("PowerupHealth");
                health += col.GetComponent<Powerup>().healthAmount;
                healthText.text = health.ToString();
                if (health > 100)
                    health = 100;
                Destroy(col.gameObject);
            }

            if (col.gameObject.GetComponent<Powerup>().powerup.ToString() == "Ammo")
            {
                AudioManager.instance.PlaySound2D("PowerUp");
                weaponController.sniperRifleAmmo += 10;
                weaponController.shotgunAmmo += 30;
                weaponController.actionRifleAmmo += 100;
                weaponController.bazookaAmmo += 2;
                weaponController.flamethrowerAmmo += 50;
                Destroy(col.gameObject);
            }            
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ShrinkDeathArea"))
        {
            if (!shrunk)
            {
                state = State.Dead;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && !invincible)
        {
            //AudioManager.instance.PlaySound2D("HitEnemy");
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

                if(col.gameObject.GetComponent<Enemy>() != null)
                PlayerTakeHit(col.gameObject.GetComponent<Enemy>().damage);

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
            PlayerTakeHit(10);
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
        shrunk = true;
        timerCanvas.SetActive(true);
        StartCoroutine("TimerShrink");
        StartCoroutine("ShrinkEffectText");
        yield return new WaitForSeconds(damageTime);
        yield return new WaitForSeconds(shrinkTime);
        UnShrink(new Vector3(1, 1, 1), startPlayerColliderSize, startPlayerColliderOffset, startplayerCircleColliderOffset, bones, collider, playerCircleCollider);
        effectText.text = "Back to being awesome!";
        shrunk = false;
        StartCoroutine("ShrinkEffectGoneTimer");
    }

    private IEnumerator ShrinkEffectText()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Shrunk! Make the best use for it!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator ShrinkEffectGoneTimer()
    {
        AudioManager.instance.PlaySound2D("PowerDown");
        effectText.gameObject.SetActive(true);
        effectText.text = "Back to being awesome!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
        timerCanvas.SetActive(false);
    }

    private IEnumerator SaveTextFader()
    {
        saveTextObject.SetActive(true);
        yield return new WaitForSeconds(2);
        saveTextObject.SetActive(false);
    }

    private IEnumerator DamageTimer()
    {
        GameObject damageEffect = Instantiate(extraDamageEffect, damageEffectPosition.transform.position, damageEffectPosition.transform.rotation) as GameObject;
        damageEffect.transform.SetParent(transform);
        berserk = true;
        timerCanvas.SetActive(true);
        damage *= 2;
        StartCoroutine("TimerDamage");
        StartCoroutine("DamageEffectText");
        yield return new WaitForSeconds(damageTime);
        damage *= 0.5f;
        Destroy(damageEffect);
        effectText.text = "Extra damage lost!";
        berserk = false;
        StartCoroutine("DamageEffectGoneTimer");
    }

    private IEnumerator DamageEffectText()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Damage Increase! BEAST MODE!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator DamageEffectGoneTimer()
    {
        AudioManager.instance.PlaySound2D("PowerDown");
        effectText.gameObject.SetActive(true);
        effectText.text = "Extra damage Lost!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
        timerCanvas.SetActive(false);
    }

    private IEnumerator InvincibilityTimer()
    {
        GameObject invincibilityEffect = Instantiate(invincibilityDamageEffect, damageEffectPosition.transform.position, damageEffectPosition.transform.rotation) as GameObject;
        invincibilityEffect.transform.SetParent(transform);
        invincible = true;
        timerCanvas.SetActive(true);
        StartCoroutine("TimerInvincibility");
        StartCoroutine("InvincibilityEffectText");
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
        Destroy(invincibilityEffect);
        StartCoroutine("InvincibilityEffectGoneTimer");
    }

    private IEnumerator InvincibilityEffectText()
    {
        effectText.gameObject.SetActive(true);
        effectText.text = "Invincible! GODSPEED!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
    }

    private IEnumerator InvincibilityEffectGoneTimer()
    {
        AudioManager.instance.PlaySound2D("PowerDown");
        effectText.gameObject.SetActive(true);
        effectText.text = "Invincibility Lost!";
        yield return new WaitForSeconds(1);
        effectText.gameObject.SetActive(false);
        timerCanvas.SetActive(false);
    }

    private IEnumerator WinScreenTimer()
    {
        yield return new WaitForSeconds(5);
        //SceneManager.LoadScene("Level2");
        //PlayerPrefs.SetFloat("PlayerPosX", 0);
        //PlayerPrefs.SetFloat("PlayerPosY", 0);
        //PlayerPrefs.SetFloat("PlayerPosZ", 0);
        //transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), PlayerPrefs.GetFloat("PlayerPosZ"));
        winPanel.SetActive(false);
    }

    private IEnumerator DeathTimer()
    {
        
        yield return new WaitForSeconds(0.7f);
        Continue();
        healthText.text = health.ToString();
        health = startHealth;
        anim.Play("Idle");
        if (!portalSpawned)
        {
            GameObject deathPortal = Instantiate(portal, transform.position, transform.rotation);
            Destroy(deathPortal, 2.5f);
            portalSpawned = true;
            AudioManager.instance.PlaySound2D("Spawn");
            StartCoroutine("DeathPortalTimer");
        }     
    }

    private IEnumerator DeathPortalTimer()
    {
        yield return new WaitForSeconds(2.5f);
        portalSpawned = false;
    }

    private IEnumerator TimerShrink()
    {
        timerCanvasText.color = new Color(128, 0, 128);
        float timer = shrinkTime;
        timerCanvasText.text = timer.ToString();
        for (int i = 0; i < shrinkTime; i++)
        {
            yield return new WaitForSeconds(1);
            timer--;
            timerCanvasText.text = timer.ToString();
            if (timer <= 3)
            {
                timerCanvasText.color = Color.red;
            }
            else
                timerCanvasText.color = new Color(128, 0, 128);
        }
        timer = shrinkTime;
    }

    private IEnumerator TimerInvincibility()
    {
        timerCanvasText.color = new Color(128, 0, 128);
        float timer = invincibilityTime;
        timerCanvasText.text = timer.ToString();
        for (int i = 0; i < invincibilityTime; i++)
        {
            yield return new WaitForSeconds(1);
            timer--;
            timerCanvasText.text = timer.ToString();
            if (timer <= 3)
            {
                timerCanvasText.color = Color.red;
            }
            else
                timerCanvasText.color = new Color(128, 0, 128);
        }
        timer = invincibilityTime;
    }

    private IEnumerator TimerDamage()
    {
        timerCanvasText.color = new Color(128, 0, 128);
        float timer = damageTime;
        timerCanvasText.text = timer.ToString();
        for (int i = 0; i < damageTime; i++)
        {
            yield return new WaitForSeconds(1);
            timer--;
            timerCanvasText.text = timer.ToString();
            if (timer <= 3)
            {
                timerCanvasText.color = Color.red;
            }
            else
                timerCanvasText.color = new Color(128, 0, 128);
        }
        timer = damageTime;
    }

    IEnumerator Dust()
    {
        yield return new WaitForSeconds(0.2f);
    }

}
