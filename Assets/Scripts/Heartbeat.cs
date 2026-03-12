using UnityEngine;
using UnityEngine.UI;

public class Heartbeat : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public GameObject enemy;

    [Header("Audio")]
    public AudioSource heartbeat;

    [Header("UI Portrait")]
    public Image portrait;
    public Sprite takut;
    public Sprite takutBanget;
    public Sprite trauma;

    void Start()
    {
        heartbeat.Play();
    }

    
    void Update()
    {
        if (enemy == null)
        {
            heartbeat.pitch = 0f;
            return;
        }

        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

        if (distance < 10f)
        {
            heartbeat.pitch = 3f;
            portrait.sprite = trauma;
        }
        else if (distance < 20f)
        {
            heartbeat.pitch = 2f;
            portrait.sprite = takutBanget;
        }
        else
        {
            heartbeat.pitch = 1f;
            portrait.sprite = takut;
        }
    }
}