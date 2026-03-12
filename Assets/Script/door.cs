using UnityEngine;
using UnityEngine.EventSystems;


public class door : MonoBehaviour {
    public PlayerInteract playerInteract;
    public AudioSource openDoorSFX;

    public GameObject closeDoor;
    
    private void Start() {
         closeDoor.SetActive(true);
    }
    private void Update() {
        if (playerInteract.taskFixCable)
        {
            closeDoor.SetActive(false);
            openDoorSFX.Play();
        }
    }
    
}