using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using System.Linq;
public class GrabObjects : NetworkBehaviour
{
    public Transform grabPoint;
    public Transform rayPoint;
    public float rayRadius;

    public GameObject grabbedObject;
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
        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.Space) && grabbedObject == null)
            {
                Debug.Log("Is Pick up");
                grabbedObject = hitInfo.collider.gameObject;
                GrabObjectServerRpc(grabbedObject.GetComponent<NetworkObject>().NetworkObjectId);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && grabbedObject != null)
            {
                // grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                // grabbedObject.GetComponent<NetworkObject>().transform.SetParent(null);
                ReleaseObjectServerRpc(grabbedObject.GetComponent<NetworkObject>().NetworkObjectId);
                grabbedObject = null;
            }
        }

        Debug.DrawLine(rayPoint.position, rayPoint.position + (Vector3)hitInfo.point, Color.white);
    }

    [ServerRpc]
    public void GrabObjectServerRpc(ulong objectID)
    {
        var boxObject = boxSpwaner.allBoxes.FirstOrDefault(iDObject =>
        iDObject.GetComponent<NetworkObject>().NetworkObjectId == objectID);
        boxObject.GetComponent<Rigidbody2D>().isKinematic = true;
        boxObject.transform.position = grabPoint.position;
        boxObject.GetComponent<NetworkObject>().transform.SetParent(transform);
    }
    [ServerRpc]
    public void ReleaseObjectServerRpc(ulong objectID)
    {
        var boxObject = boxSpwaner.allBoxes.FirstOrDefault(iDObject =>
        iDObject.GetComponent<NetworkObject>().NetworkObjectId == objectID);
        boxObject.GetComponent<Rigidbody2D>().isKinematic = false;
        boxObject.GetComponent<NetworkObject>().transform.SetParent(null);
        
    }

}
