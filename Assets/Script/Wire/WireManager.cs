 using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
        closedAsset.SetActive(true);
        openAsset.SetActive(false);

        foreach (GameObject wire in wirePairs) {
            wire.SetActive(false);
        }

        if (timerText != null)
            timerText.text = "";
    }

    public void OpenAsset() {
        closedAsset.SetActive(false);
        openAsset.SetActive(true);

        foreach (GameObject wire in wirePairs) {
            wire.SetActive(true);
        }

        // mulai timer sekali
        timer = timeLimit;
        started = true;

        Debug.Log("Asset terbuka, semua kabel muncul! Timer dimulai.");
    }

    void Update() {
        // cek input spasi → hanya buka asset kalau belum mulai
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !started) {
            OpenAsset();
        }

        // update timer setiap frame
        if (started && !Done) {
            timer -= Time.deltaTime;

            if (timerText != null) {
                timerText.text = "Waktu: " + Mathf.Ceil(timer).ToString();
            }

            if (timer <= 0f) {
                started = false;
                Debug.Log("Waktu habis!");
                if (timerText != null)
                    timerText.text = "Waktu habis!";
            }
        }
    }

    public void WireConnected() {
        connectedPairs++;

        if (connectedPairs >= totalPairs && !Done) {
            Done = true;
            Debug.Log("Semua kabel tersambung, task completed!");
            if (timerText != null)
                timerText.text = "Task Completed!";
        }
    }
}