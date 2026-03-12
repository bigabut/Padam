using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D flashlight;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineTargetGroup targetGroup;

    public Transform defaultTrack;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomInSize = 3f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeAmount = 5f;

    [Header("Battery Settings")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryDrain = 10f;

    [Header("Flicker Settings")]
    [SerializeField] private AnimationCurve flickerCurve;
    [SerializeField] private float flickerRangeDetail = 0.2f;
    [SerializeField] private float flickerIntensity = 1f;

    public AudioSource click;
    public AudioSource flashlightEffect;

    private float currentBattery;

    private CinemachineBasicMultiChannelPerlin noise;
    private float defaultSize;

    private bool monsterInside = false;
    private Transform currentMonster;

    private float baseIntensity;

    void Awake()
    {
        flashlight = GetComponent<Light2D>();

        noise = cinemachineCamera
            .GetCinemachineComponent(CinemachineCore.Stage.Noise)
            as CinemachineBasicMultiChannelPerlin;

        defaultSize = cinemachineCamera.Lens.OrthographicSize;

        currentBattery = maxBattery;

        baseIntensity = flashlight.intensity;
    }

    void Start()
    {
        defaultTrack = cinemachineCamera.Follow;

        targetGroup.AddMember(defaultTrack, 2f, 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentBattery > 0)
        {
            flashlight.enabled = !flashlight.enabled;
            click.Play();
        }

        HandleBattery();

        bool effectActive = false;

        if (monsterInside && flashlight.enabled && HasLineOfSight())
        {
            effectActive = true;
        }

        // AUDIO hanya aktif jika benar-benar terlihat
        if (effectActive)
        {
            if (!flashlightEffect.isPlaying)
                flashlightEffect.Play();
        }
        else
        {
            if (flashlightEffect.isPlaying)
                flashlightEffect.Stop();
        }

        // Camera Shake
        if (noise != null)
            noise.AmplitudeGain = effectActive ? shakeAmount : 0f;

        // Camera Zoom
        float targetSize = effectActive ? zoomInSize : defaultSize;

        cinemachineCamera.Lens.OrthographicSize =
            Mathf.Lerp(
                cinemachineCamera.Lens.OrthographicSize,
                targetSize,
                zoomSpeed * Time.deltaTime
            );
    }

    void HandleBattery()
    {
        if (!flashlight.enabled) return;

        currentBattery -= batteryDrain * Time.deltaTime;

        if (currentBattery <= 0)
        {
            currentBattery = 0;
            flashlight.enabled = false;
            return;
        }

        float flicker = GetFlickerAmount();

        flashlight.intensity = baseIntensity + flicker;
    }

    private float GetFlickerAmount()
    {
        float t = flickerCurve.Evaluate(1 - currentBattery / maxBattery);

        float flickerAmount =
            Random.Range(-flickerRangeDetail, flickerRangeDetail)
            * t
            / flickerRangeDetail
            * flickerIntensity;

        return flickerAmount;
    }

    bool HasLineOfSight()
    {
        if (currentMonster == null) return false;

        Vector2 direction = currentMonster.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction.normalized,
            distance,
            wallLayer
        );

        return hit.collider == null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            monsterInside = true;
            currentMonster = other.transform;

            targetGroup.AddMember(other.transform, 1f, 1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            monsterInside = false;
            currentMonster = null;

            targetGroup.RemoveMember(other.transform);
        }
    }
}