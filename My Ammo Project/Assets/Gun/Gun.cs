using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Diagnostics;
using System.Linq;
public class Gun : NetworkBehaviour
{
    public float damage;
    public int currentAmmo;
    public int currentAmmoPistol;
    public int currentAmmoShotgun;
    public int currentAssaultRifle;
    public int maxAmmo;
    public List<GameObject> spawnedBullet = new List<GameObject>();
    public bool isReload;
    public float reloadSpeed;
    public GameObject bulletPrefab;
    public float bulletspeed = 100f;
    public Vector2 direction;
    public Coroutine reloadgun;
    public WeaponType weaponType;
    public AmmoManager ammoManager;
    private void Start()
    {
        ammoManager = GameObject.FindObjectOfType<AmmoManager>();
        InitializeWeapon();
    }
    void Update()
    {

        if (!IsOwner) return;

        // AimAtMouseServerRpc();
        ChangeWeaponServerRpc();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            ShootBulletServerRpc(direction);

            if (currentAmmo > 0)
            {
                // หยุดการรีโหลด
                if (reloadgun != null)
                {
                    StopCoroutine(reloadgun);
                    reloadgun = null;
                }
                isReload = false;
             ammoManager.UpdateAmmoData(OwnerClientId, weaponType, currentAmmo);
            }
        }
        if (Input.GetKey(KeyCode.R) && !isReload)
        {
            reloadgun = StartCoroutine(ReloadGun());
        }


    }
    [ServerRpc]
    void ShootBulletServerRpc(Vector2 direction)
    {

        if (this.currentAmmo > 0)
        {
            currentAmmo--;
            GameObject bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
            spawnedBullet.Add(bullet);

            if (weaponType == WeaponType.Shotgun)
            {

            }

            Rigidbody2D rbbullet = bullet.GetComponent<Rigidbody2D>();

            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = this.damage;
            bulletScript.gun = this;

            rbbullet.AddForce(direction * bulletspeed, ForceMode2D.Impulse);
            bullet.GetComponent<NetworkObject>().Spawn();
        }
        else if (!isReload)
        {
            reloadgun = StartCoroutine(ReloadGun());

        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc(ulong networkObjectID)
    {
        GameObject bulletDestroy = spawnedBullet.FirstOrDefault(iDObject =>
        iDObject.GetComponent<NetworkObject>().NetworkObjectId == networkObjectID);

        if (bulletDestroy is null) return;

        bulletDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBullet.Remove(bulletDestroy);
        Destroy(bulletDestroy);
    }
    [ServerRpc]
    void ChangeWeaponServerRpc()
    {
        if (Input.GetKey(KeyCode.Alpha3))
        {
            weaponType = WeaponType.Pistol;
            InitializeWeapon();
            ammoManager.UpdateAmmoData(OwnerClientId, weaponType, currentAmmo);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            weaponType = WeaponType.Shotgun;
            InitializeWeapon();
            ammoManager.UpdateAmmoData(OwnerClientId, weaponType, currentAmmo);
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            weaponType = WeaponType.AssaultRifle;
            InitializeWeapon();
            ammoManager.UpdateAmmoData(OwnerClientId, weaponType, currentAmmo);
        }
    }
    void InitializeWeapon()
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                damage = 45;
                maxAmmo = 7;
                currentAmmo = maxAmmo;
                reloadSpeed = 3.0f;
                break;
            case WeaponType.Shotgun:
                damage = 70;
                maxAmmo = 4;
                currentAmmo = maxAmmo;
                reloadSpeed = 1.0f;
                break;
            case WeaponType.AssaultRifle:
                damage = 45;
                maxAmmo = 30;
                currentAmmo = maxAmmo;
                reloadSpeed = 2.5f;
                break;
        }
    }

    public IEnumerator ReloadGun()
    {
        isReload = true;
        if (weaponType != WeaponType.Shotgun)
        {
            yield return new WaitForSeconds(reloadSpeed);
            currentAmmo = maxAmmo;
            ammoManager.UpdateAmmoData(OwnerClientId, weaponType, currentAmmo);
        }
        else
        {
            isReload = true;
            yield return new WaitForSeconds(reloadSpeed);
            currentAmmo++;
            if (currentAmmo < maxAmmo) { }
            StartCoroutine(ReloadGun());
        }
        isReload = false;
    }
}
public enum WeaponType { Pistol, Shotgun, AssaultRifle }
