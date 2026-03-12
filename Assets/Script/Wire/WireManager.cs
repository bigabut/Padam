using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class WireManager : MonoBehaviour, IPointerDownHandler {
    [Header("Asset Panel")]
    public GameObject closedAsset;
    public GameObject openAsset;
    public GameObject hintPaper;
    public PlayerInteract playerInteract;

    [Header("Wire Pairs")]
    public CableLines[] wireLines;   // isi sesuai jumlah kabel (misalnya 4)
    public CableEnds[] wireEnds;     // isi sesuai jumlah port (misalnya 8)

    public int connectedPairs = 0;
    public int totalPairs = 4;
    public bool Done = false;

    [Header("Timer")]
    public float timeLimit = 15f;
    public float timer;
    private bool started = false;
    private bool openBox = false;
    public AudioSource cableOnTargetSFX;
    [Header("UI")]
    public TMP_Text timerText;
    public GameObject pintuFixKabel;

    void Start() {
        timer = timeLimit;
        if (closedAsset != null) closedAsset.SetActive(true);
        if (openAsset != null) openAsset.SetActive(false);

        foreach (var line in wireLines) {
            if (line != null) line.gameObject.SetActive(false);
        }

        started = true;
        UpdateTimerText();
    }

    void Update() {
        if (started && !Done) {
            if (timer > 0 && connectedPairs < totalPairs) {
                timer -= Time.unscaledDeltaTime;
                UpdateTimerText();
            }
 
            if (timer <= 0f && !Done) {
                Debug.Log("Waktu habis!");
                connectedPairs = 0;

                // Reset semua kabel dengan aman
                foreach (var line in wireLines) {
                    if (line != null) {
                        line.ResetCable();

                    }
                }

                playerInteract.CloseFixCableUILose();
            }

            if (connectedPairs >= totalPairs) {
                Done = true;
                foreach (var line in wireLines) {
                    if (line != null) line.gameObject.SetActive(false);
                }
                if (timerText != null) timerText.text = "Menang!";
            }
        }
    }

    void UpdateTimerText() {
        if (timerText != null) {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!openBox) {
            OpenAsset();
            Debug.Log("Klik Canvas → buka asset");
            openBox = true;
        }
    }

    public void OpenAsset() {
        if (closedAsset != null) closedAsset.SetActive(false);
        if (openAsset != null) openAsset.SetActive(true);

        foreach (var line in wireLines) {
            if (line != null) line.gameObject.SetActive(true);
        }

        Debug.Log("Asset terbuka, semua kabel muncul!");
    }

    public void WireConnected() {
        cableOnTargetSFX.Play();
        connectedPairs++;
        Debug.Log("Kabel terhubung! Total: " + connectedPairs);

        if (connectedPairs >= totalPairs) {
            Done = true;
            if (timerText != null) timerText.text = "Menang!";
            playerInteract.CloseFixCableUI();
            pintuFixKabel.SetActive(false);
        }
    }
}