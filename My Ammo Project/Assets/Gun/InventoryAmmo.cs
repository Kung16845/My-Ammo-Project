using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InventoryAmmo : NetworkBehaviour
{
    public int reserveAmmoPistol;
     public GameObject bulletPistolprefab;
    public int reserveAmmoAssaultRifle;
     public GameObject bulletAssaultRifleprefab;
    public int reserveAmmoShotgun;
     public GameObject bulletShotgunprefab;
   
}
