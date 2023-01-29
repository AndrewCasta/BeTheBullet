using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public GameObject WeaponHolster;
    public GunController equiptWeapon;
    public int startingWeapon;

    private void Start()
    {
        SelectWeapon(1);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        equiptWeapon.Shoot(context);
    }

    private void SelectWeapon(int weaponIndex)
    {
        var weapons = WeaponHolster.GetComponentsInChildren<GunController>();
        equiptWeapon = weapons[weaponIndex];
        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(weapon == equiptWeapon);

    }

}
