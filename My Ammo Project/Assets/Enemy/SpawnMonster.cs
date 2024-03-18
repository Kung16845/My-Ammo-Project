using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
public class SpawnMonster : NetworkBehaviour
{
    public float duration;
    Coroutine spawnCoroutine;
    public List<GameObject> prefabMonster;
    public List<GameObject> spawnedMonsters = new List<GameObject>();
    private void Start()
    {   
        StartCoroutine(SpawnMon());
    }
    public void StopSpawnEnemy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    IEnumerator SpawnMon()
    {
        yield return new WaitForSeconds(duration);
        SpawnMonsterServerRpc();
        spawnCoroutine = StartCoroutine(SpawnMon());
    }
    [ServerRpc]
    public void SpawnMonsterServerRpc()
    {
        var transformSpawnMonster = new Vector3(this.transform.position.x, Random.Range(-4.0f, 4.0f));
        int n = Random.Range(0, 2);
        GameObject monster = Instantiate(prefabMonster[n], transformSpawnMonster, this.transform.rotation);

        spawnedMonsters.Add(monster);
        monster.GetComponent<Enemy>().spawnMonster = this;
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
