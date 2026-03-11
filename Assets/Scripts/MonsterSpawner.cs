using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    public float respawnTime = 60f;

    public void StartRespawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        Transform spawnPoint = GetFarthestPoint();

        GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);

        EnemyAi ai = monster.GetComponent<EnemyAi>();
        ai.target = player;
        ai.spawner = this;
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