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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;

        ulong networkObjectIDBullet = GetComponent<NetworkObject>().NetworkObjectId;

        if (other.gameObject.tag == "Wall")
        {
            gun.DestroyBulletServerRpc(networkObjectIDBullet);
        }
        if (other.gameObject.tag == "Enemy")
        {
            var enemyComponent = other.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamageEnemyServerRpc(damage);
            }
            gun.DestroyBulletServerRpc(networkObjectIDBullet);
        }
    }


}
