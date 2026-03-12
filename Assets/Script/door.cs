using UnityEngine;
using UnityEngine.EventSystems;


public class door : MonoBehaviour {
    public PlayerInteract playerInteract;
    public AudioSource openDoorSFX;

    public GameObject openDoor;
    public GameObject closeDoor;

    private void Start() {
        openDoor.SetActive(false);
        closeDoor.SetActive(true);
        openDoorSFX.Stop();
    }
    private void Update() {
        if (playerInteract.taskFixCable)
        {
            openDoor.SetActive(true);
            closeDoor.SetActive(false);
            openDoorSFX.Play();
        }
    }
    
}