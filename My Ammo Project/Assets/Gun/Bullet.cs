using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Bullet : NetworkBehaviour
{
    public float damage;
    public Vector2 startLocal;
    private void Awake()
    {
        startLocal = new Vector2(transform.position.x, transform.position.y);
    }
    private void Update()
    {
        if (IsOwner)
        {
            var currenposition = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(startLocal, currenposition) > 20)
            {
                DestroyBulletServerRpc();
            }
        }
    }

    [ServerRpc]
    public void DestroyBulletServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

}
