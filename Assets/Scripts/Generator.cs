using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Generator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image startProgressBar;
    public Image taskProgressBar;
    public TextMeshProUGUI timerText;

    public float holdStartRequired = 2f;   // waktu tahan untuk buka generator
    public float holdTimeRequired = 3f;    // waktu tahan untuk isi task
    public float missionTime = 15f;        // total waktu misi

    public PlayerInteract01 playerInteract;

    private float startTimer = 0f;
    private float holdTimer = 0f;
    private float missionTimer = 0f;

    private bool generatorOpened = false;
    private bool taskCompleted = false;
    private bool timesout = false;
    private bool isHolding = false;

    public AudioSource audioSource;
    public AudioSource fillOil;

    void Start()
    {
            audioSource.Stop();
            fillOil.Stop();
        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;

        missionTimer = missionTime;
        UpdateTimerText();
    }

    void Update()
    {
        // Timer mundur
        if(!taskCompleted)
        {
            missionTimer = Mathf.Max(0, missionTimer - Time.unscaledDeltaTime);
            UpdateTimerText();
            if(missionTimer <= 0f) FailMission();
        }

        // Logika hold
        if(isHolding)
        {
            if(!generatorOpened && !timesout)
            {
                startTimer += Time.unscaledDeltaTime;
                if(startProgressBar != null)
                    startProgressBar.fillAmount = startTimer / holdStartRequired;

                
                   

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
                holdTimer += Time.unscaledDeltaTime;
                if(taskProgressBar != null)
                    taskProgressBar.fillAmount = holdTimer / holdTimeRequired;

                if(holdTimer >= holdTimeRequired)
                {
                    CompleteTask();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        Debug.Log("Mulai tahan klik UI");
         if(!generatorOpened && !timesout)
            {
                        audioSource.Play();

                   

                if(startTimer >= holdStartRequired)
                {
                   
                    audioSource.Stop();
                }
            }
            else if(generatorOpened && !taskCompleted)
            {
                    fillOil.Play();
            }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        startTimer = 0f;
        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        audioSource.Stop();
        fillOil.Stop();
        Debug.Log("Lepas klik UI");
    }

    void StartMission()
    {
        holdTimer = 0f;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
    }

    void CompleteTask()
    {
        taskCompleted = true;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 1f;
        if(timerText != null) timerText.text = "Task Complete!";
        Debug.Log("Generator fixed!");
        audioSource.Stop();
        fillOil.Stop();
        playerInteract.CloseGeneratorUI();
    }

    void FailMission()
    {
        generatorOpened = false;
        taskCompleted = false;
        timesout = true;
        if(timerText != null) timerText.text = "Failed!";
        if(startProgressBar != null) startProgressBar.fillAmount = 0;
        if(taskProgressBar != null) taskProgressBar.fillAmount = 0;
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