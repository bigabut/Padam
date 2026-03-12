using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    public Transform target;
    public MonsterSpawner spawner;
    public AudioSource lari;

    [Header("Raycast Settings")]
    public LayerMask wallLayer;

    private NavMeshAgent agent;
    private SpriteRenderer sprite;
    private Light2D flashLight;

    float normalSpeed = 5f;
    float slowSpeed = 1f;

    float flashlightTimer = 0f;
    float flashlightThreshold = 2f;

    bool inFlashlight = false;
    bool triggered = false;

    Transform flashlightSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        sprite = GetComponent<SpriteRenderer>();
        flashLight = GetComponentInChildren<Light2D>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = normalSpeed;

        if (flashLight != null)
            flashLight.intensity = 0f;

        Debug.Log("Monster Spawned");
    }

    void Update()
    {
        if (target != null && !triggered)
        {
            agent.SetDestination(target.position);
        }

        HandleFlashlight();
    }

    void HandleFlashlight()
    {
        if (inFlashlight && flashlightSource != null && !triggered)
        {
            Vector2 direction = transform.position - flashlightSource.position;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(
                flashlightSource.position,
                direction.normalized,
                distance,
                wallLayer
            );

            Debug.DrawRay(flashlightSource.position, direction.normalized * distance, Color.blue);

            if (hit.collider == null)
            {
                flashlightTimer += Time.deltaTime;
                agent.speed = slowSpeed;

                Debug.Log("Monster kena cahaya");

                if (flashlightTimer >= flashlightThreshold)
                {
                    triggered = true;
                    StartCoroutine(FlashbangEffect());
                }
            }
            else
            {
                Debug.Log("Cahaya terhalang tembok");
            }
        }
        else
        {
            if (!triggered)
                agent.speed = normalSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            inFlashlight = true;
            flashlightSource = other.transform;

            Debug.Log("Monster masuk area flashlight");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            inFlashlight = false;
            flashlightTimer = 0f;
            flashlightSource = null;

            Debug.Log("Monster keluar dari flashlight");
        }
    }

    IEnumerator FlashbangEffect()
    {
        agent.isStopped = true;

        sprite.color = Color.white;

        if (flashLight != null)
            flashLight.intensity = 25f;

        yield return new WaitForSeconds(0.15f);

        if (flashLight != null)
            flashLight.intensity = 0f;

        yield return new WaitForSeconds(0.1f);

        lari.Play();

        Despawn();
    }

    void Despawn()
    {
        if (spawner != null)
        {
            spawner.StartRespawn();
        }

        Destroy(gameObject);
    }
}