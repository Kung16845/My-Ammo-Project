using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
public class BagAmmo : NetworkBehaviour
{
  
    public bool isOpening;
    public bool isPlayerNear;
    public InventoryAmmo inventoryAmmo;
    public int ammorefill;
    public WeaponType weaponType;

    private void Update()
    {
        if(!IsServer) return;

        // if (isPlayerNear )
        // {
        //     OpenBagServerRpc();
        // }

        // if (isOpening)
        // {
        //     RefillAmmoServerRpc();
        // }
    }
    [ServerRpc(RequireOwnership = false)]
    public void RefillAmmoServerRpc(ServerRpcParams rpcParams = default)
    {

        
            if (weaponType == WeaponType.Pistol)
            {
                if (inventoryAmmo.currentReserveAmmoPistol + ammorefill <= inventoryAmmo.maxReserveAmmoPistol)
                {
                    inventoryAmmo.currentReserveAmmoPistol += ammorefill;
                    ammorefill = 0;
                }
                else
                {
                    ammorefill = ammorefill - (inventoryAmmo.maxReserveAmmoPistol - inventoryAmmo.currentReserveAmmoPistol);
                    inventoryAmmo.currentReserveAmmoPistol = inventoryAmmo.maxReserveAmmoPistol;

                }
            }
            else if (weaponType == WeaponType.Shotgun)
            {
                if (inventoryAmmo.currentReserveAmmoShotgun + ammorefill <= inventoryAmmo.maxReserveAmmoShotgun)
                {
                    inventoryAmmo.currentReserveAmmoShotgun += ammorefill;
                    ammorefill = 0;
                }
                else
                {
                    ammorefill = ammorefill - (inventoryAmmo.maxReserveAmmoShotgun - inventoryAmmo.currentReserveAmmoShotgun);
                    inventoryAmmo.currentReserveAmmoShotgun = inventoryAmmo.maxReserveAmmoShotgun;

                }

            }
            else if (weaponType == WeaponType.AssaultRifle)
            {
                if (inventoryAmmo.currentReserveAmmoAssaultRifle + ammorefill <= inventoryAmmo.maxReserveAmmoAssaultRifle)
                {
                    inventoryAmmo.currentReserveAmmoAssaultRifle += ammorefill;
                    ammorefill = 0;
                }
                else
                {
                    ammorefill = ammorefill - (inventoryAmmo.maxReserveAmmoAssaultRifle - inventoryAmmo.currentReserveAmmoAssaultRifle);
                    inventoryAmmo.currentReserveAmmoAssaultRifle = inventoryAmmo.maxReserveAmmoAssaultRifle;
                }
            }
        
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.gameObject.tag == "Player")
        {
            isPlayerNear = true;
            inventoryAmmo = other.GetComponent<InventoryAmmo>();
            // other.GetComponent<PlayerScrpt>().bagAmmo = this;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsServer) return;

        if (other.gameObject.tag == "Player")
        {
            isPlayerNear = false;
            inventoryAmmo = null;
            // other.GetComponent<PlayerScrpt>().bagAmmo = null;
        }
    }
}
