using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHold;
    public Transform weaponHold2;
    public Transform weaponHold3;
    public Weapon startingWeapon;
    public Weapon[] weapons;
    public Weapon equippedWeapon;
    public GameObject ammoPanel1;
    public GameObject ammoPanel2;
    public GameObject ammoPanel3;

    public enum Weapons {SniperRifle, Shotgun};
    public Weapons weapon;

    [Range(1, 100)]
    public int projectileSpeed = 30;

    int i = 0;

    Weapon ws; //weapon script

    public Image weaponImage;
    public Text ammoText;

    public Sprite gunImage;
    public Sprite sniperRifleImage;
    public Sprite shotgunImage;
    public Sprite actionRifleImage;
    public Sprite bazookaImage;
    public Sprite flamethrowerImage;

    public Weapon weaponToEquip;

    public string gunAmmo;
    public int sniperRifleAmmo;
    public int shotgunAmmo;
    public int actionRifleAmmo;
    public int bazookaAmmo;
    public int flamethrowerAmmo;

    public float damage;
    int weaponNumber;
    Vector2 originalScale;
    PlayerScript ps;

    private void Start()
    {
        EquipWeapon(startingWeapon);
        ws = equippedWeapon.GetComponent<Weapon>();
        ps = GameObject.Find("Player").GetComponent<PlayerScript>();
        weaponToEquip = equippedWeapon;
        gunAmmo = "∞";
        sniperRifleAmmo = 100;
        shotgunAmmo = 100;
        actionRifleAmmo = 300;
        bazookaAmmo = 5;
        flamethrowerAmmo = 200;
        originalScale = new Vector2(3, 3);
    }

    private void Update()
    {        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            weaponNumber += 1;
            if (weaponNumber >= weapons.Length)
                weaponNumber = 0;
            EquipWeapon(weapons[weaponNumber]);
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            weaponNumber -= 1;
            if (weaponNumber < 0)
                weaponNumber = weapons.Length -1;
            EquipWeapon(weapons[weaponNumber]);           
        }

        if (!ps.shrunk)
        {
            weapons[0].transform.localScale = new Vector2(2f, 1.8f );
            weapons[1].transform.localScale = originalScale;
            weapons[2].transform.localScale = originalScale;
            weapons[3].transform.localScale = originalScale;
            weapons[4].transform.localScale = new Vector2(4, 4);
            weapons[5].transform.localScale = originalScale;
        }

        else if (ps.shrunk)
        {
            weapons[0].transform.localScale = new Vector2(1, 1);
            weapons[1].transform.localScale = new Vector2(1, 1);
            weapons[2].transform.localScale = new Vector2(1, 1);
            weapons[3].transform.localScale = new Vector2(1, 1);
            weapons[4].transform.localScale = new Vector2(1, 1);
            weapons[5].transform.localScale = new Vector2(1, 1);
        }


        foreach (Transform m in ammoPanel1.transform)
        {
            if (m.name == "GunAmmoText")
            {
                m.GetComponent<Text>().text = gunAmmo;
            }
            if (m.name == "SniperRifleAmmoText")
            {
                m.GetComponent<Text>().text = sniperRifleAmmo.ToString();
            }
        }
        foreach (Transform m in ammoPanel2.transform)
        {
            if (m.name == "ShotgunAmmoText")
            {
                m.GetComponent<Text>().text = shotgunAmmo.ToString();
            }
            if (m.name == "ActionRifleAmmoText")
            {
                m.GetComponent<Text>().text = actionRifleAmmo.ToString();
            }
        }
        foreach (Transform m in ammoPanel3.transform)
        {
            if (m.name == "BazookaAmmoText")
            {
                m.GetComponent<Text>().text = bazookaAmmo.ToString();
            }
            if (m.name == "FlamethrowerAmmoText")
            {
                m.GetComponent<Text>().text = flamethrowerAmmo.ToString();
            }
        }

        if (equippedWeapon.name == weapons[0].name+ "(Clone)")
            ammoText.text = gunAmmo.ToString();
        else if (equippedWeapon.name == weapons[1].name + "(Clone)")
            ammoText.text = sniperRifleAmmo.ToString();
        else if (equippedWeapon.name == weapons[2].name + "(Clone)")
            ammoText.text = shotgunAmmo.ToString();
        else if (equippedWeapon.name == weapons[3].name + "(Clone)")
            ammoText.text = actionRifleAmmo.ToString();
        else if (equippedWeapon.name == weapons[4].name + "(Clone)")
            ammoText.text = bazookaAmmo.ToString();
        else if (equippedWeapon.name == weapons[5].name + "(Clone)")
            ammoText.text = flamethrowerAmmo.ToString();
    }

    public void EquipWeapon(Weapon weaponToEquip)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        if (weaponToEquip != weapons[4])
        {
            equippedWeapon = Instantiate(weaponToEquip, weaponHold.position, weaponHold.rotation) as Weapon;
            equippedWeapon.transform.parent = weaponHold;

        }
        else
        {
            equippedWeapon = Instantiate(weaponToEquip, weaponHold2.position, weaponHold2.rotation) as Weapon;
            equippedWeapon.transform.parent = weaponHold2;
        }
        
        if (weaponToEquip.name == weapons[0].name)
        {
            weaponImage.GetComponent<Image>().sprite = gunImage;
        }
        if (weaponToEquip.name == weapons[1].name)
        {
            weaponImage.GetComponent<Image>().sprite = sniperRifleImage;          
        }
        if (weaponToEquip.name == weapons[2].name)
        {
            weaponImage.GetComponent<Image>().sprite = shotgunImage;
        }
        if (weaponToEquip.name == weapons[3].name)
        {
            weaponImage.GetComponent<Image>().sprite = actionRifleImage;
        }
        if (weaponToEquip.name == weapons[4].name)
        {
            weaponImage.GetComponent<Image>().sprite = bazookaImage;
        }
        if (weaponToEquip.name == weapons[5].name)
        {
            weaponImage.GetComponent<Image>().sprite = flamethrowerImage;
        }
    }

    public void OnTriggerHold()
    {
        if (equippedWeapon != null)
            equippedWeapon.OnTriggerHold();
    }

    public void OnTriggerRelease()
    {
        if(equippedWeapon != null)
        {
            equippedWeapon.OnTriggerRelease();
        }
    }
}
