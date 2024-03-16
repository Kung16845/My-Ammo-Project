using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public float currentHp;
    public float maxHp;
    public Vector2 direction;
    public Transform target;
    public float speed;
    public float currentspeed;
    public Rigidbody2D rb;
    public float damage;
    public float attackTimer;
    public float countTImer;
    public SpawnMonster spawnMonster;

    void Start()
    {
        var objBarrier = FindObjectOfType<Barrier>().gameObject;
        // pointBarrier = new Vector2(objBarrier.transform.position.x, objBarrier.transform.position.y);
        target = objBarrier.transform;
        
        currentspeed = speed;
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {   
        var objBarrier = other.GetComponent<Barrier>();
        // Debug.Log(objBarrier);
        if(objBarrier != null)
        {   
            
            objBarrier.TakeDamageBarrierServerRpc(damage);
            this.GetComponent<Collider2D>().enabled = false;
        }

    }
    
    [ServerRpc]
    public void TakeDamageEnemyServerRpc(float damage)
    {
        this.currentHp -= damage;

        if (this.currentHp <= 0)
        {
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
            spawnMonster.DestroyMonsterServerRpc(networkObjectID);
        }

    }
}
