using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEngine.UI;
public class PlayerScrpt : NetworkBehaviour
{
    public float currentHp;
    public float maxHp;
    public float holdTime = 2f;
    public float timer = 0f;
    public BagAmmo bagAmmo;
    public BoxAmmoSpwaner boxSpwaner;
    public Slider timerBar;
    private void Awake()
    {
        boxSpwaner = FindObjectOfType<BoxAmmoSpwaner>();
        timerBar = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
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
        UpdateTimerUI();

        if (bagAmmo != null && bagAmmo.isOpening.Value)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bagAmmo.RefillAmmoServerRpc();
            }
        }

    }
    void UpdateTimerUI()
    {
        if (timer != 0 && !bagAmmo.isOpening.Value)
        {
            timerBar.gameObject.SetActive(true);
            float ratio = timer / holdTime;
            timerBar.value = ratio;
        }
        else
        {
            timerBar.gameObject.SetActive(false);
        }

    }
    [ServerRpc(RequireOwnership = false)]
    public void OpenBagServerRpc(ulong bagAmmoNetworkObjectId, ServerRpcParams rpcParams = default)
    {
        var bagAmmoNetworkObject = boxSpwaner.allBoxes.FirstOrDefault(bag => bag.GetComponent<NetworkObject>().NetworkObjectId == bagAmmoNetworkObjectId);

        if (bagAmmoNetworkObject != null)
        {
            var bagAmmo = bagAmmoNetworkObject.GetComponent<BagAmmo>();
            bagAmmo.isOpening.Value = true;
        }

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
