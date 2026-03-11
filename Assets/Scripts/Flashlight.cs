using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Cinemachine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D flashlight;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomInSize = 3f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeAmount = 5f;

    [Header("Obstacle Layer")]
    [SerializeField] private LayerMask wallLayer;

    [Header("Battery Settings")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float batteryDrain = 10f;

    [Header("Flicker Settings")]
    [SerializeField] private AnimationCurve flickerCurve;
    [SerializeField] private float flickerRangeDetail = 0.2f;
    [SerializeField] private float flickerIntensity = 1f;

    private float currentBattery;

    private CinemachineBasicMultiChannelPerlin noise;
    private float defaultSize;

    private bool monsterInside = false;
    private Transform monsterTransform;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentBattery > 0)
        {
            flashlight.enabled = !flashlight.enabled;
        }

        HandleBattery();

        bool effectActive = false;

        if (monsterInside && flashlight.enabled && monsterTransform != null)
        {
            Vector2 direction = monsterTransform.position - transform.position;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction.normalized,
                distance,
                wallLayer
            );

            if (!hit)
            {
                effectActive = true;
            }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            monsterInside = true;
            monsterTransform = other.transform;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("monster"))
        {
            monsterInside = false;
            monsterTransform = null;
        }
    }
}