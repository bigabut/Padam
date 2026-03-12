using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    
    public AudioSource ngejar;

    public float respawnTime = 60f;

    public void StartRespawn()
    {
        StartCoroutine(RespawnRoutine());
        
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        Transform spawnPoint = GetFarthestPoint();

        if (spawnPoint == null)
        {
            Debug.LogError("Tidak ada spawn point!");
            yield break;
        }

        NavMeshHit hit;

        if (NavMesh.SamplePosition(spawnPoint.position, out hit, 2f, NavMesh.AllAreas))
        {
            GameObject monster = Instantiate(monsterPrefab, hit.position, Quaternion.identity);

            EnemyAi ai = monster.GetComponent<EnemyAi>();
            ai.target = player;
            ai.spawner = this;
            ngejar.Play();
            Debug.Log("Monster respawn di NavMesh");
        }
        else
        {
            Debug.LogError("SpawnPoint tidak berada di NavMesh!");
        }
    }

    Transform GetFarthestPoint()
    {
        Transform farthest = null;
        float maxDistance = 0f;

        foreach (Transform point in spawnPoints)
        {
            float dist = Vector2.Distance(point.position, player.position);

            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthest = point;
            }
        }

        return farthest;
    }
}