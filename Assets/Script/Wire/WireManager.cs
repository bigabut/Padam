using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class WireManager : MonoBehaviour {
    [Header("Asset Panel")]
    public GameObject closedAsset;
    public GameObject openAsset;

    [Header("Wire Pairs")]
    public GameObject[] wirePairs;

    private int connectedPairs = 0;
    public int totalPairs = 4;
    public bool Done = false;

    [Header("Timer")]
    public float timeLimit = 15f;   // durasi waktu (detik)
    private float timer;
    private bool started = false;

    [Header("UI")]
    public TMP_Text timerText;   // drag TMP Text dari Canvas ke sini

    void Start() {
        timer = timeLimit;
        closedAsset.SetActive(true);
        openAsset.SetActive(false);

        foreach (GameObject wire in wirePairs) {
            wire.SetActive(false);
        }

        if (timerText != null)
            timerText.text = "";
 
        started = false;
    }

    void Update() {
       

        // update timer setiap frame, pakai unscaledDeltaTime
         
            if(timer>0 &&connectedPairs <4 )
            timer -= Time.unscaledDeltaTime;

            if (timerText != null) {
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                

            if (timer <= 0f) {
                started = false;
                Debug.Log("Waktu habis!");
                if (timerText != null)
                    timerText.text = "Waktu habis!";
                EndTask(false);
            }
        
            if(connectedPairs >=4)
            timerText.text = "Task Completed";

             // buka asset dengan klik kiri mouse
            if (Mouse.current.leftButton.wasPressedThisFrame && !openAsset.activeSelf) {
            OpenAsset();
        }
        }
    }

    public void OpenAsset() {
        closedAsset.SetActive(false);
        openAsset.SetActive(true);

        foreach (GameObject wire in wirePairs) {
            wire.SetActive(true);
        }

        // mulai timer di sini
        started = true;

        Debug.Log("Asset terbuka, semua kabel muncul!");
    }

    public void WireConnected() {
        connectedPairs++;

        if (connectedPairs >= totalPairs  ) {
            Done = true;
          
            EndTask(true);
        }
    }

    private void EndTask(bool success) {
        started = false;
        foreach (GameObject wire in wirePairs) {
            wire.SetActive(false);
        }
       

        
    }
}