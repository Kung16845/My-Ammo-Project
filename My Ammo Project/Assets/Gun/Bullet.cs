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
        startLocal = new Vector2(transform.position.x, transform.position.y);

    }
    private void Update()
    {
        if (!IsOwner) { return; }

        var currenposition = new Vector2(this.transform.position.x, this.transform.position.y);
        if (Vector2.Distance(startLocal, currenposition) > 20)
        {
            gun.DestroyBulletServerRpc(GetComponent<NetworkObject>().NetworkObjectId);
        }
    }



}
