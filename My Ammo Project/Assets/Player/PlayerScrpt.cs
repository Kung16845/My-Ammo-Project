using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerScrpt : NetworkBehaviour
{
    public float currentHp;
    public float maxHp;
    [ServerRpc]
    public void TakeDamagePlayerServerRpc(float damage)
    {
        this.currentHp -= damage;

        if (this.currentHp <= 0)
        {
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
            
        }

    }
}
