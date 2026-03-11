using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        audioSource.Play(); // atau PlayOneShot(audioSource.clip);
    }
}