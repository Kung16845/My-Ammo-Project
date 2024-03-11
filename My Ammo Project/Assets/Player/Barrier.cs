using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float currentHpBarrier;
    public float maxHpBarrier;
    private void Start() {
        currentHpBarrier = maxHpBarrier;
    }
    [ServerRpc]
    public void TakeDamageBarrierServerRpc(float danage)
    {   
        currentHpBarrier -= danage;
        if(currentHpBarrier <= 0)
        {
            // DestroyBarrierServerRpc();
        } 
    }

    [ServerRpc(RequireOwnership = false)]
    void DestroyBarrierServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

}
