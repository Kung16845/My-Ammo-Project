using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
public class BoxAmmoSpwaner : NetworkBehaviour
{
    public List<GameObject> allBoxes;
    public List<GameObject> prefebBagAmmo;
    public List<Transform> listtransformsSpawnedBagAmmo;

    [ServerRpc]
    public void SpawnedBagAmmoServerRpc()
    {
        GameObject newAmmoBag = prefebBagAmmo.FirstOrDefault(weaponTypeObj => weaponTypeObj.GetComponent<BagAmmo>().weaponType == WeaponType.Pistol);

        GameObject ammoBagaSpawed = Instantiate(newAmmoBag, this.transform.position, this.transform.rotation);
        allBoxes.Add(ammoBagaSpawed);
        ammoBagaSpawed.GetComponent<NetworkObject>().Spawn(true);
    }
}
