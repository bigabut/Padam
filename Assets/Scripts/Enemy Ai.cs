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
    private Light2D flashLight;
    private Animator animator;

    float normalSpeed = 5f;
    float slowSpeed = 1f;

    float flashlightTimer = 0f;
    float flashlightThreshold = 2f;

    bool inFlashlight = false;
    bool triggered = false;

    Transform flashlightSource;

    Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        if (triggered) return;

        if (target != null)
            agent.SetDestination(target.position);

        UpdateAnimationDirection();

        HandleFlashlight();
    }

    void UpdateAnimationDirection()
    {
        Vector2 velocity = agent.velocity;

        if (velocity.magnitude > 0.1f)
        {
            lastMoveDir = velocity.normalized;
        }

        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }

    void HandleFlashlight()
    {
        if (!inFlashlight || flashlightSource == null)
        {
            agent.speed = normalSpeed;
            flashlightTimer = 0f;
            return;
        }

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

            if (flashlightTimer >= flashlightThreshold)
            {
                triggered = true;
                StartCoroutine(FlashbangEffect());
            }
        }
        else
        {
            flashlightTimer = 0f;
            agent.speed = normalSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            inFlashlight = true;
            flashlightSource = other.transform.root;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("flashlight"))
        {
            inFlashlight = false;
            flashlightTimer = 0f;
            flashlightSource = null;
        }
    }

    IEnumerator FlashbangEffect()
    {
        agent.isStopped = true;

        if (flashLight != null)
            flashLight.intensity = 25f;

        yield return new WaitForSeconds(0.15f);

        if (flashLight != null)
            flashLight.intensity = 0f;

        yield return new WaitForSeconds(0.1f);

        if (lari != null)
            lari.Play();

        Despawn();
    }

    void Despawn()
    {
        if (spawner != null)
            spawner.StartRespawn();

        Destroy(gameObject);
    }
}