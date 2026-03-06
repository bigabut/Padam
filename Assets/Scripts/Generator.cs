using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratorTask : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Settings")]
    public float holdStartRequired = 2f;   // Waktu hold untuk buka generator
    public float holdTimeRequired = 3f;    // Waktu hold untuk task
    public float missionTime = 15f;        // Waktu misi aktif

    [Header("UI")]
    public Image startProgressBar;  // Circle fill untuk hold start
    public Image taskProgressBar;   // Vertical fill untuk task
    public TextMeshProUGUI timerText;

    private SpriteRenderer sr;
    private bool generatorOpened = false;
    private bool taskCompleted = false;
    private float startTimer = 0f;
    private float holdTimer = 0f;
    private float missionTimer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(sr == null) Debug.LogError("SpriteRenderer tidak ditemukan!");
        sr.sprite = closedSprite;

        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;

        // --- Timer langsung muncul saat Play
        missionTimer = missionTime;
        if(timerText != null)
        {
            int minutes = Mathf.FloorToInt(missionTimer / 60f);
            int seconds = Mathf.FloorToInt(missionTimer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseOver = Physics2D.OverlapPoint(mousePos) == GetComponent<Collider2D>();

        // --- Update mission timer selalu berjalan jika generator sudah dibuka & task belum selesai
        if(!taskCompleted)
        {
            missionTimer = Mathf.Max(0, missionTimer - Time.deltaTime);

            if(timerText != null)
            {
                int minutes = Mathf.FloorToInt(missionTimer / 60f);
                int seconds = Mathf.FloorToInt(missionTimer % 60f);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            if(missionTimer <= 0f)
                FailMission();
        }

        // --- Hitung hold click
        if(Input.GetMouseButton(0) && mouseOver)
        {
            if(!generatorOpened)
            {
                // Hold untuk buka generator
                startTimer += Time.deltaTime;
                if(startProgressBar != null)
                    startProgressBar.fillAmount = startTimer / holdStartRequired;

                if(startTimer >= holdStartRequired)
                {
                    generatorOpened = true;
                    StartMission();
                    startTimer = 0f;
                    if(startProgressBar != null) startProgressBar.fillAmount = 0;
                }
            }
            else if(generatorOpened && !taskCompleted)
            {
                // Hold untuk task
                holdTimer += Time.deltaTime;
                if(taskProgressBar != null)
                    taskProgressBar.fillAmount = holdTimer / holdTimeRequired;

                if(holdTimer >= holdTimeRequired)
                    CompleteTask();
            }
        }
        else
        {
            // Reset timers jika mouse dilepas atau bukan di object
            startTimer = 0f;
            holdTimer = 0f;
            if(startProgressBar != null) startProgressBar.fillAmount = 0;
            if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
        }
    }

    void StartMission()
    {
        sr.sprite = openSprite;
        missionTimer = missionTime;
        holdTimer = 0f;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
    }

    void CompleteTask()
    {
        taskCompleted = true;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 1f;
        if(timerText != null) timerText.text = "Task Complete!";
        Debug.Log("Generator fixed!");
    }

    void FailMission()
    {
        generatorOpened = false;
        taskCompleted = false;
        sr.sprite = closedSprite;
        if(timerText != null) timerText.text = "Failed!";
        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
    }
}