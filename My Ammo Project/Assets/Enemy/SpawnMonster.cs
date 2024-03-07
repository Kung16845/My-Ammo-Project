using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
public class SpawnMonster : NetworkBehaviour
{
    public float duration;
    public GameObject prefabMonster;
    public List<GameObject> spawnedMonsters = new List<GameObject>();
    private void Start()
    {
        // StartCoroutine(SpawnMon());    
    }
    IEnumerator SpawnMon()
    {
        yield return new WaitForSeconds(duration);
        SpawnMonsterServerRpc();
        StartCoroutine(SpawnMon());
    }
    [ServerRpc]
    public void SpawnMonsterServerRpc()
    {
        var transformSpawnMonster = new Vector3(this.transform.position.x, Random.Range(-15.0f, 15.0f));
        GameObject monster = Instantiate(prefabMonster, transformSpawnMonster, this.transform.rotation);
        
        spawnedMonsters.Add(monster);
        monster.GetComponent<Monster>().spawnMonster = this;
        monster.GetComponent<NetworkObject>().Spawn();
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyMonsterServerRpc(ulong networkObjectID)
    {
        GameObject monsterDestroy = spawnedMonsters.FirstOrDefault(iDObject =>
       iDObject.GetComponent<NetworkObject>().NetworkObjectId == networkObjectID);

        if (monsterDestroy is null) return;
        monsterDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedMonsters.Remove(monsterDestroy);
        Destroy(monsterDestroy);
    }
}
