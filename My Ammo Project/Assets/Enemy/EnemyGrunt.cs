using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyGrunt : Enemy
{
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
                this.GetComponent<Collider2D>().enabled = true;

                countTImer = attackTimer;
            }
        }

    }

}
