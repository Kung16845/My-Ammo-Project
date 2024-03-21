using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class PlayerScrpt : NetworkBehaviour
{
    public float currentHp;
    public float maxHp;
    public float holdTime = 2f;
    public float timer = 0f;
    public BagAmmo bagAmmo;
    public BoxAmmoSpwaner boxSpwaner;
    private void Awake()
    {
        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
    }
    private void Update()
    {
        if (!IsOwner) return;
        
        if (bagAmmo != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                timer += Time.deltaTime;
                if (timer >= holdTime)
                {
                    ulong bagAmmoiDObject = bagAmmo.GetComponent<NetworkObject>().NetworkObjectId;
                    OpenBagServerRpc(bagAmmoiDObject);
                }
            }

            else
                timer = 0;
        }
        if (bagAmmo != null && bagAmmo.isOpening)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bagAmmo.RefillAmmoServerRpc();
            }
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void OpenBagServerRpc(ulong iDObjectbagAmmoNear, ServerRpcParams rpcParams = default)
    {
        var bagAmmo = boxSpwaner.allBoxes.FirstOrDefault(bagid => bagid.GetComponent<NetworkObject>().NetworkObjectId ==
             iDObjectbagAmmoNear).GetComponent<BagAmmo>();
        bagAmmo.isOpening = true;
        // if (Input.GetKey(KeyCode.E))
        // {
        //     timer += Time.deltaTime;
        //     if (timer >= holdTime)
        //     {
        //         bagAmmo.isOpening = true;
        //     }
        // }

        // else
        //     timer = 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsOwner) return;


        if (other.GetComponent<BagAmmo>() != null)
        {

            this.bagAmmo = other.GetComponent<BagAmmo>();

        }

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsOwner) return;
        this.bagAmmo = null;

    }
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
