using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    public Transform target;
    public MonsterSpawner spawner;

    [SerializeField] Transform[] runPoints;

    NavMeshAgent agent;

    float normalSpeed = 5f;
    float slowSpeed = 1f;

    float flashlightTimer = 0f;
    float flashlightThreshold = 2f;

    bool isFleeing = false;
    bool inFlashlight = false;

    SpriteRenderer sprite;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = normalSpeed;
    }

    void Update()
    {
        if (!isFleeing)
        {
            agent.SetDestination(target.position);
        }

        HandleFlashlight();
    }

    void HandleFlashlight()
    {
        if (inFlashlight && !isFleeing)
        {
            flashlightTimer += Time.deltaTime;

            agent.speed = slowSpeed;

            if (flashlightTimer >= flashlightThreshold)
            {
                flashlightTimer = 0f;
                StartCoroutine(FlashlightEffect());
            }
        }
        else
        {
            flashlightTimer = 0f;

            if (!isFleeing)
            {
                agent.speed = normalSpeed;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("flashlight") && !isFleeing)
        {
            inFlashlight = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("flashlight") && !isFleeing)
        {
            inFlashlight = false;
        }
    }

    IEnumerator FlashlightEffect()
    {
        isFleeing = true;
        inFlashlight = false;

        // efek flash kesakitan
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sprite.color = Color.white;

        yield return new WaitForSeconds(0.2f);

        // monster panik lari cepat
        agent.speed = 20f;

        Transform fleePoint = GetFarthestPoint();

        if (fleePoint != null)
        {
            agent.SetDestination(fleePoint.position);
            StartCoroutine(CheckArrival(fleePoint));
        }
    }

    Transform GetFarthestPoint()
    {
        Transform farthest = null;
        float maxDistance = 0f;

        foreach (Transform point in runPoints)
        {
            float dist = Vector2.Distance(point.position, target.position);

            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthest = point;
            }
        }

        return farthest;
    }

    IEnumerator CheckArrival(Transform point)
    {
        while (Vector2.Distance(transform.position, point.position) > 0.5f)
        {
            yield return null;
        }

        spawner.StartRespawn();

        Destroy(gameObject);
    }
}