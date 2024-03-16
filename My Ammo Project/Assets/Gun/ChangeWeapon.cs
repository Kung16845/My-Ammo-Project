using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ChangeWeapon : NetworkBehaviour
{
    public Gun gun;
    public int currentAmmoPistol;
    public int currentAmmoShotgun;
    public int currentAmmoAssaultRifle;
    private void Start()
    {
        InitializeWeapon();
    }
    private void Update()
    {
        if (!IsOwner) { return; }

        ChangeTypeWeapon();
    }

    public void ChangeTypeWeapon()
    {

        if (Input.GetKey(KeyCode.Alpha3))
        {
            SavecurrentAmmoServerRpc(gun.weaponType);
            gun.weaponType = WeaponType.Pistol;

            UpdateWeaponType(gun.weaponType);

            // gun.ammoManager.UpdateAmmoData(OwnerClientId, gun.weaponType, gun.currentAmmo);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SavecurrentAmmoServerRpc(gun.weaponType);
            gun.weaponType = WeaponType.Shotgun;

            UpdateWeaponType(gun.weaponType);

            // gun.ammoManager.UpdateAmmoData(OwnerClientId, gun.weaponType, gun.currentAmmo);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            SavecurrentAmmoServerRpc(gun.weaponType);
            gun.weaponType = WeaponType.AssaultRifle;

            UpdateWeaponType(gun.weaponType);
            // gun.ammoManager.UpdateAmmoData(OwnerClientId, gun.weaponType, gun.currentAmmo);
        }
    }
    [ServerRpc]
    void SavecurrentAmmoServerRpc(WeaponType weaponType)
    {
        if (weaponType == WeaponType.AssaultRifle)
            currentAmmoAssaultRifle = gun.currentAmmo;
        else if (weaponType == WeaponType.Shotgun)
            currentAmmoShotgun = gun.currentAmmo;
        else
            currentAmmoPistol = gun.currentAmmo;
    }
    void UpdateWeaponType(WeaponType newWeaponType)
    {
        // ส่งข้อมูลประเภทปืนไปยัง client
        UpdateWeaponTypeServerRpc(newWeaponType);

        // อัปเดตปืนใน server โดยตรง
        gun.weaponType = newWeaponType;
        InitializeWeapon();
    }

    [ServerRpc]
    void UpdateWeaponTypeServerRpc(WeaponType newWeaponType)
    {
        // อัปเดตปืนใน client
        gun.weaponType = newWeaponType;

        InitializeWeapon();
    }
    void InitializeWeapon()
    {
        switch (gun.weaponType)
        {
            case WeaponType.Pistol:
                gun.damage = 45;
                gun.maxAmmo = 7;
                gun.currentAmmo = currentAmmoPistol;
                gun.reloadSpeed = 3.0f;
                break;
            case WeaponType.Shotgun:
                gun.damage = 70;
                gun.maxAmmo = 4;
                gun.currentAmmo = currentAmmoShotgun;
                gun.reloadSpeed = 1.0f;
                break;
            case WeaponType.AssaultRifle:
                gun.damage = 45;
                gun.maxAmmo = 30;
                gun.currentAmmo = currentAmmoAssaultRifle;
                gun.reloadSpeed = 2.5f;
                break;
        }
    }

}
