using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpeedter : Enemy
{   

    public bool isFirstAttack;

    void Update()
    {
        if (!IsServer) return;

        direction = (target.position - this.transform.position).normalized;
        rb.velocity = direction * currentspeed;

        if (!this.GetComponent<Collider2D>().enabled)
        {
            if (countTImer > 0)
                countTImer -= Time.deltaTime;
            else
            {
                if (isFirstAttack){
                    damage = damage * 2;
                    isFirstAttack = false;
                    damage = damage / 2;
                }
                    

                this.GetComponent<Collider2D>().enabled = true;

                countTImer = attackTimer;
            }

        }
    }
    
}
