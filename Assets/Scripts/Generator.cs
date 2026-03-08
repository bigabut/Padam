 using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Generator : MonoBehaviour
{
   
    public Sprite closedSprite;
    public Sprite openSprite;

 
    public float holdStartRequired = 2f;
    public float holdTimeRequired = 3f;
    public float missionTime = 15f;
    public PlayerInteract playerInteract;

 
    public Image startProgressBar;
    public Image taskProgressBar;
    public TextMeshProUGUI timerText;
    public Button testButton;  

    private SpriteRenderer sr;
    private bool generatorOpened = false;
    private bool taskCompleted = false;
    private float startTimer = 0f;
    private float holdTimer = 0f;
    private float missionTimer = 0f;
    private bool timesout = false;

    public AudioSource audioSource;
    public AudioSource fillOil;

    


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
            missionTimer = Mathf.Max(0, missionTimer - Time.unscaledDeltaTime);
            UpdateTimerText();
            if(missionTimer <= 0f) FailMission();
        }

        if(Input.GetMouseButton(0) && mouseOver)
        {
            if(!generatorOpened && !timesout)
            {
                startTimer += Time.unscaledDeltaTime;
               
                if(startProgressBar != null)
                    startProgressBar.fillAmount = startTimer / holdStartRequired;
                
                if(!audioSource.isPlaying && startTimer > 0.1f)
                {
                    audioSource.Play(); 
                }


                if(startTimer >= holdStartRequired)
                {
                    generatorOpened = true;
                    StartMission();
                    startTimer = 0f;
                    if(startProgressBar != null) startProgressBar.fillAmount = 0;
                    audioSource.Stop();

                }
            }
            else if(generatorOpened && !taskCompleted)
            {
                 fillOil.Play();   
                 holdTimer += Time.unscaledDeltaTime;
                if(taskProgressBar != null)
                    taskProgressBar.fillAmount = holdTimer / holdTimeRequired;

                if(holdTimer >= holdTimeRequired)
                    CompleteTask();
                    fillOil.Stop();
            }
        }
        else
        {
            startTimer = 0f;
            if(startProgressBar != null) startProgressBar.fillAmount = 0;
            audioSource.Stop();
            fillOil.Stop();

         }
    }
    
    void StartMission()
    {
        sr.sprite = openSprite;
        holdTimer = 0f;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
    }

    void CompleteTask()
    {
        taskCompleted = true;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 1f;
        if(timerText != null) timerText.text = "Task Complete!";
        Debug.Log("Generator fixed!");

        if(taskProgressBar != null) taskProgressBar.fillAmount = 1f;
         if(timerText != null) timerText.text = "Task Complete!";
        Debug.Log("Generator fixed!");

        // Balik ke main game UI
        playerInteract.CloseGeneratorUI();
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