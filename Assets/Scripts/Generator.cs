using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Generator : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Settings")]
    public float holdStartRequired = 2f;
    public float holdTimeRequired = 3f;
    public float missionTime = 15f;

    [Header("UI")]
    public Image startProgressBar;
    public Image taskProgressBar;
    public TextMeshProUGUI timerText;
    public Button testButton; // contoh tombol di canvas

    private SpriteRenderer sr;
    private bool generatorOpened = false;
    private bool taskCompleted = false;
    private float startTimer = 0f;
    private float holdTimer = 0f;
    private float missionTimer = 0f;
    private bool timesout = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(sr == null) Debug.LogError("SpriteRenderer tidak ditemukan!");
        sr.sprite = closedSprite;

        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;

        missionTimer = missionTime;
        UpdateTimerText();
    }

   

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool mouseOver = Physics2D.OverlapPoint(mousePos) == GetComponent<Collider2D>();

        if(!taskCompleted)
        {
            missionTimer = Mathf.Max(0, missionTimer - Time.deltaTime);
            UpdateTimerText();
            if(missionTimer <= 0f) FailMission();
        }

        if(Input.GetMouseButton(0) && mouseOver)
        {
            if(!generatorOpened && !timesout)
            {
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
                holdTimer += Time.deltaTime;
                if(taskProgressBar != null)
                    taskProgressBar.fillAmount = holdTimer / holdTimeRequired;

                if(holdTimer >= holdTimeRequired)
                    CompleteTask();
            }
        }
        else
        {
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
        timesout = true;
        sr.sprite = closedSprite;
        if(timerText != null) timerText.text = "Failed!";
        startProgressBar.fillAmount = 0;
        taskProgressBar.fillAmount = 0;
    }

    void UpdateTimerText()
    {
        if(timerText != null)
        {
            int minutes = Mathf.FloorToInt(missionTimer / 60f);
            int seconds = Mathf.FloorToInt(missionTimer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    
}