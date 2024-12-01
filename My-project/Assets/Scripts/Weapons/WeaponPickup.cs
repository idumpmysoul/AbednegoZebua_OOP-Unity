using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weaponHolder;

    Weapon weapon;

    // Spawn WeaponHolder Object when the game start
    void Awake()
    {
        if (weaponHolder != null)
            weapon = Instantiate(weaponHolder);
    }

    // Set default value for weaponHolder
    void Start()
    {
        // if no weaponHolder provided, dont do anything
        if (weapon == null)
            return;

        // TurnVisual so the weapon sprites doesnt crash with weapon pickup sprites
        TurnVisual(false);

        // Turn off the functionality of weapon
        weapon.enabled = false;
        // Make the weapon object to be children of weaponHolder
        weapon.transform.SetParent(transform, false);
        // Match the parent positions
        weapon.transform.localPosition = transform.position;

        // save transform to reset the position
        weapon.parentTransform = transform;
    }

    // Ini kalo misal si objek weapon pickup "kena" objek Player, masukin slot weapon yang ada di weaponHolder ke slot Weapon si Player
    void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger itu bisa ke semua objek, jadi harus cari si Player
        if (weapon != null && other.gameObject.CompareTag("Player"))
        {
            Weapon playerWeapon = other.gameObject.GetComponentInChildren<Weapon>();

            // Kalo misal slot Weapon si Player udah penuh (udah ada Weapon)
            // Maka tuker Weapon yang baru disentuh sama Weapon yang ada di slot
            // Kalo mau liat cara kerjanya bisa tambahin aja objek WeaponPickup
            // di WeaponRack terus bedain posisi antar dua Weapon
            if (playerWeapon != null)
            {
                playerWeapon.transform.SetParent(playerWeapon.parentTransform);
                playerWeapon.transform.localScale = new(1, 1);
                playerWeapon.transform.localPosition = new(0, 0);

                TurnVisual(false, playerWeapon);
            }

            weapon.enabled = true;
            weapon.transform.SetParent(other.transform, false);

            TurnVisual(true);

            weapon.transform.localPosition = new(0.0f, 0.0f);
        }
    }

    void TurnVisual(bool on)
    {
        if (on)
        {
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            weapon.GetComponent<Animator>().enabled = true;
            weapon.GetComponent<Weapon>().enabled = true;
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().enabled = false;
            weapon.GetComponent<Animator>().enabled = false;
            weapon.GetComponent<Weapon>().enabled = false;
        }

    }

    void TurnVisual(bool on, Weapon weapon)
    {
        if (on)
        {
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            weapon.GetComponent<Animator>().enabled = true;
            weapon.GetComponent<Weapon>().enabled = true;
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().enabled = false;
            weapon.GetComponent<Animator>().enabled = false;
            weapon.GetComponent<Weapon>().enabled = false;
        }

    }
}