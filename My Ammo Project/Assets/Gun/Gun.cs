using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Diagnostics;
public class Gun : NetworkBehaviour
{
    public enum WeaponType { Pistol, Shotgun, AssaultRifle }
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
    public WeaponType weaponType;
    private void Start()
    {
        InitializeWeapon();
    }
    void Update()
    {

        if (!IsOwner) return;

        // AimAtMouseServerRpc();
        ChangeWeapon();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootBulletServerRpc();
            if (currentAmmo > 0)
            {
                StopAllCoroutines(); // หยุดการรีโหลด
                isReload = false;
            }
        }
        if (Input.GetKey(KeyCode.R) && !isReload)
        {
            StartCoroutine(ReloadGun());
        }


    }
    
    [ServerRpc]
    private void AimAtMouseServerRpc()
    {
        
    }
    [ServerRpc]
    void ShootBulletServerRpc()
    {  
                
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;                
    
        if (this.currentAmmo > 0)
        {
            currentAmmo--;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            spawnedBullet.Add(bullet);
            Rigidbody2D rbbullet = bullet.GetComponent<Rigidbody2D>();

            var bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = this.damage;

            rbbullet.AddForce(direction * bulletspeed, ForceMode2D.Impulse);
            bullet.GetComponent<NetworkObject>().Spawn(true);
        }
        else if (!isReload)
        {
            StartCoroutine(ReloadGun());
        }
    }
    void ChangeWeapon()
    {
        if (Input.GetKey(KeyCode.Alpha3))
        {
            weaponType = WeaponType.Pistol;
            InitializeWeapon();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            weaponType = WeaponType.Shotgun;
            InitializeWeapon();
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            weaponType = WeaponType.AssaultRifle;
            InitializeWeapon();
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
        }
        else
        {
            isReload = true;
            yield return new WaitForSeconds(reloadSpeed);
            currentAmmo++;
            if (currentAmmo < maxAmmo)
                StartCoroutine(ReloadGun());
        }
        isReload = false;
    }
}
