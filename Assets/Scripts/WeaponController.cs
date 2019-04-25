using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour, IShootAmmo
{
    public Transform weaponHold;
    public Transform weaponHold2;
    public Weapon startingWeapon;
    public Weapon[] weapons;
    public Weapon equippedWeapon;
    public GameObject ammoPanel;
    
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

    public Weapon weaponToEquip;

    public int gunAmmo;
    public int sniperRifleAmmo;
    public int shotgunAmmo;
    public int actionRifleAmmo;
    public int bazookaAmmo;

    public float damage;

    private void Start()
    {
        EquipWeapon(startingWeapon);
        ws = equippedWeapon.GetComponent<Weapon>();
        weaponToEquip = equippedWeapon;
        gunAmmo = 100;
        sniperRifleAmmo = 100;
        shotgunAmmo = 100;
        actionRifleAmmo = 300;
        bazookaAmmo = 5;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Debug.Log("hi");

            EquipWeapon(weapons[i]);
            

            i++;
            if (i >= weapons.Length)
                i = 0;
        }

        foreach(Transform m in ammoPanel.transform)
        {
            if(m.name == "GunAmmoText")
            {
                m.GetComponent<Text>().text = gunAmmo.ToString();
            }
            if (m.name == "SniperRifleAmmoText")
            {
                m.GetComponent<Text>().text = sniperRifleAmmo.ToString();
            }
            if (m.name == "ShotgunAmmoText")
            {
                m.GetComponent<Text>().text = shotgunAmmo.ToString();
            }
            if (m.name == "ActionRifleAmmoText")
            {
                m.GetComponent<Text>().text = actionRifleAmmo.ToString();
            }
            if (m.name == "BazookaAmmoText")
            {
                m.GetComponent<Text>().text = bazookaAmmo.ToString();
            }
        }

        if (equippedWeapon.name == weapons[3].name+ "(Clone)")
            ammoText.text = gunAmmo.ToString();
        else if (equippedWeapon.name == weapons[0].name + "(Clone)")
            ammoText.text = sniperRifleAmmo.ToString();
        else if (equippedWeapon.name == weapons[1].name + "(Clone)")
            ammoText.text = shotgunAmmo.ToString();
        else if (equippedWeapon.name == weapons[2].name + "(Clone)")
            ammoText.text = actionRifleAmmo.ToString();
        else if (equippedWeapon.name == weapons[4].name + "(Clone)")
            ammoText.text = bazookaAmmo.ToString();
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
       

        if (weaponToEquip.name == weapons[3].name)
        {
            weaponImage.GetComponent<Image>().sprite = gunImage;
        }
        if (weaponToEquip.name == weapons[0].name)
        {
            weaponImage.GetComponent<Image>().sprite = sniperRifleImage;          
        }
        if (weaponToEquip.name == weapons[1].name)
        {
            weaponImage.GetComponent<Image>().sprite = shotgunImage;
        }
        if (weaponToEquip.name == weapons[2].name)
        {
            weaponImage.GetComponent<Image>().sprite = actionRifleImage;
        }
        if (weaponToEquip.name == weapons[4].name)
        {
            weaponImage.GetComponent<Image>().sprite = bazookaImage;
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

    public void DepleteAmmo(Weapon weapon)
    {
        if(weapon == equippedWeapon)
        {
            Debug.Log("depleted ammo from " + weapon);
        }
    }
}
