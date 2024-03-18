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
            OpenBagServerRpc();
    }
    [ServerRpc]
    public void OpenBagServerRpc(ServerRpcParams rpcParams = default)
    {
        if (Input.GetKey(KeyCode.E))
        {
            timer += Time.deltaTime;
            if (timer >= holdTime)
            {
                bagAmmo.isOpening = true;
            }
        }

        else
            timer = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsOwner)
        {

            if (other.GetComponent<BagAmmo>() != null)
            {
                var bag = boxSpwaner.allBoxes.FirstOrDefault(bagid => bagid.GetComponent<NetworkObject>().NetworkObjectId ==
                 other.GetComponent<NetworkObject>().NetworkObjectId);
                this.bagAmmo = bag.GetComponent<BagAmmo>();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsOwner)
        {
            this.bagAmmo = null;
        }
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
