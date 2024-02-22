using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class Movement : NetworkBehaviour
{
    public float speed = 5f;

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            float vertical = Input.GetAxis("Vertical") ;
            float horizontal = Input.GetAxis("Horizontal") ;
            
            Vector2 direction = new Vector2(horizontal, vertical);
            transform.Translate(direction * speed  * Time.deltaTime);
        }
    }
}
