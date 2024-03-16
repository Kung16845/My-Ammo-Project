using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InventoryAmmo : NetworkBehaviour
{
    public int currentReserveAmmoPistol;
    public int maxReserveAmmoPistol;
    public int currentReserveAmmoAssaultRifle;
    public int macReserveAmmoAssaultRifle;
    public int currentReserveAmmoShotgun;
    public int MaxReserveAmmoShotgun;

    public void RefillAmmo(WeaponType weaponType,int ammorefill)
    {
        if(weaponType == WeaponType.Pistol)
        {
            currentReserveAmmoPistol -= ammorefill;
        }
        else if(weaponType == WeaponType.Shotgun)
        {
            currentReserveAmmoShotgun -= ammorefill;
        }
        else if(weaponType == WeaponType.AssaultRifle)
        {
            currentReserveAmmoAssaultRifle -= ammorefill;
        }
    }
    
}
