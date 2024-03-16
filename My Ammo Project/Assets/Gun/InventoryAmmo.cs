using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class InventoryAmmo : NetworkBehaviour
{
    public int currentReserveAmmoPistol;
    public int maxReserveAmmoPistol;
    public int currentReserveAmmoAssaultRifle;
    public int maxReserveAmmoAssaultRifle;
    public int currentReserveAmmoShotgun;
    public int maxReserveAmmoShotgun;
    public Gun gun;
    private void Start()
    {
        currentReserveAmmoAssaultRifle = maxReserveAmmoAssaultRifle;
        currentReserveAmmoShotgun = maxReserveAmmoShotgun;
        currentReserveAmmoPistol = maxReserveAmmoPistol;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RefillAmmoServerRpc(WeaponType weaponType, int ammorefill)
    {
        if (weaponType == WeaponType.Pistol)
        {
            currentReserveAmmoPistol -= ammorefill;
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            currentReserveAmmoShotgun -= ammorefill;
        }
        else if (weaponType == WeaponType.AssaultRifle)
        {
            currentReserveAmmoAssaultRifle -= ammorefill;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void CheckCurrentReserveAmmoServerRpc(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Pistol)
        {
            gun.isCanReload = currentReserveAmmoPistol > 0 ? true : false;
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            gun.isCanReload = currentReserveAmmoShotgun > 0 ? true : false;
        }
        else if (weaponType == WeaponType.AssaultRifle)
        {
            gun.isCanReload = currentReserveAmmoAssaultRifle > 0 ? true : false;
        }
    }
}
