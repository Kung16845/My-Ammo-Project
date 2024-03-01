using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class Gun : NetworkBehaviour
{
    public float damage;
    public int currentInMags;
    public int maxMags;
    public GameObject bulletPrefab;
    public float bulletspeed = 10f;
    public Vector2 direction;

    void Update()
    {
        if (IsOwner)
        {
            AimAtMouse();
            if (Input.GetButtonDown("Fire1")) // ตรวจสอบว่ามีการคลิกปุ่มซ้ายของเมาส์หรือไม่
            {
                ShootBulletServerRpc();
                
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                TestChagecolorBulletClientRpc();
            }
        }
    }

    private void AimAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;
    }
    [ServerRpc] 
    void ShootBulletServerRpc()
    {   
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        bullet.GetComponent<NetworkObject>().Spawn();
        rb.AddForce(direction * bulletspeed, ForceMode2D.Impulse);
    }
    [ClientRpc]
    void TestChagecolorBulletClientRpc()
    {   
        if (!IsOwner) // ตรวจสอบว่าเป็น Owner ของปืนหรือไม่
        {
            bulletPrefab.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }
}
