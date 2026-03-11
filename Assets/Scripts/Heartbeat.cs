using UnityEngine;
using UnityEngine.AI;
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

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

        if (enemy != null)
        {
            // MUSUH SANGAT DEKAT
            if (distance < 10f)
            {
                heartbeat.pitch = 3f;
                portrait.sprite = trauma;
            }

            // MUSUH MENDEKAT
            else if (distance < 20f)
            {
                heartbeat.pitch = 2f;
                portrait.sprite = takutBanget;
            }

            // AMAN
            else
            {
                heartbeat.pitch = 1f;
                portrait.sprite = takut;
            }
        }

        else
        {
            heartbeat.pitch = 0f;
        }
    }
}