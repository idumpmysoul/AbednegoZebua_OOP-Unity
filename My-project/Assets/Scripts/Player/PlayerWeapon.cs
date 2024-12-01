using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    Weapon weapon;

    public Weapon SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        return weapon;
    }
}
