using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class RaycastBagAmmo : NetworkBehaviour
{
    public Transform grabPoint;
    public Transform rayPoint;
    public float rayRadius = 2.5f;
    public float holdTime = 2f;
    public float timer = 0f;
    private LayerMask objectLayer;
    public BoxAmmoSpwaner boxSpwaner;
    private void Start()
    {
        objectLayer = LayerMask.GetMask("Objects");
        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        RaycastHit2D hitInfo = Physics2D.CircleCast(rayPoint.position, rayRadius, transform.right, rayRadius, objectLayer);
        if (hitInfo.collider.GetComponent<BagAmmo>() != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;
                if (timer >= holdTime)
                {
                    hitInfo.collider.GetComponent<BagAmmo>().isOpening.Value = true;
                }
            }

            else
                timer = 0;
        }
    }
}
