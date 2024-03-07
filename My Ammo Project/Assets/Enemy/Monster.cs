using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Monster : NetworkBehaviour
{   
    public float currentHp;
    public float maxHp;
    public Vector2 direction;
    public Transform pointBarrier;
    public float speed;
    public float currentspeed;
    public Rigidbody2D rb;
    public SpawnMonster spawnMonster;

    void Start()
    {
        var objBarrier = FindObjectOfType<Barrier>().gameObject;
        // pointBarrier = new Vector2(objBarrier.transform.position.x, objBarrier.transform.position.y);
        pointBarrier = objBarrier.transform;
        currentspeed = speed;
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {   
        if(!IsServer) return;
        direction = (pointBarrier.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed ;

        //*ใช้งานได้
        // float step = currentspeed * Time.deltaTime;
        // transform.position = Vector3.MoveTowards(transform.position, pointBarrier.position, step);
    }
    [ServerRpc ]
    public void TakeDamageServerRpc(float damage)
    {
        this.currentHp -= damage;

        if(this.currentHp <= 0)
        {   
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
            spawnMonster.DestroyMonsterServerRpc(networkObjectID);
        }

    }
}
