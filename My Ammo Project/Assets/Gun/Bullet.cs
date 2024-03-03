using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Bullet : NetworkBehaviour
{
    public float damage;
    public Vector2 startLocal;
    public Gun gun;

    private void Awake()
    {
        // startLocal = new Vector2(transform.position.x, transform.position.y);

    }
    // private void Update()
    // {
    //     if (!IsOwner) { return; }

    //     var currenposition = new Vector2(this.transform.position.x, this.transform.position.y);
    //     if (Vector2.Distance(startLocal, currenposition) > 20.0F)
    //     {
    //         // gun.DestroyBulletServerRpc(GetComponent<NetworkObject>().NetworkObjectId);
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;

        ulong networkObjectIDBullet = GetComponent<NetworkObject>().NetworkObjectId;

        if (other.gameObject.tag == "Wall")
        {
            gun.DestroyBulletServerRpc(networkObjectIDBullet);
        }
        if(other.gameObject.tag == "Enemy")
        {   
            // other.GetComponent<Monster>().TakeDamageServerRpc(damage);
            other.GetComponent<Monster>().spawnMonster.DestroyMonsterServerRpc(other.GetComponent<NetworkObject>().NetworkObjectId);
            gun.DestroyBulletServerRpc(networkObjectIDBullet);
        }
    }


}
