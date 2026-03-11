using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class PinGame : MonoBehaviour
{
    public Text displayText;              // Text biasa
    private bool isCorrect = false;       
    public string correctPIN = "1234";    // PIN yang benar
    private string inputPIN = "";         
    public TextMeshProUGUI timerText;     
    public bool isRunning = true;          // 1 = true, 0 = false
    public float timeRemaining = 15f;   
    public AudioSource audioSFX;  


    void Start()
    {
       

    if (timerText == null)
        timerText = GetComponent<TextMeshProUGUI>();

    if (displayText == null)
        displayText = GetComponent<Text>();

    if (audioSFX == null)
        audioSFX = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (isRunning)
        {
          
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeRemaining / 60);   // bagi 60
                int seconds = Mathf.FloorToInt(timeRemaining % 60);   // sisa bagi 60

                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                
                isRunning = false;

                timeRemaining = 0f;
                timerText.text = "Waktu Habis!";
            }
        }
    }

    public void OnKeyPress(string key)
    {
        if (isRunning)
        {
            audioSFX.Play();

            if (key == "#") // submit
            {

                if (inputPIN == correctPIN)
                {
                    displayText.text = "BENAR";
                    isCorrect = true;
                    isRunning = false; // stop timer
                }
                else
                {
                    displayText.text = "SALAH";
                }
                inputPIN = "";
            }
            else if (key == "*") // reset
            {
                inputPIN = "";
                displayText.text = "Reset PIN";
            }
            else if (key == "DEL") // delete
            {
                if (inputPIN.Length > 0)
                {
                    inputPIN = inputPIN.Substring(0, inputPIN.Length - 1);
                    displayText.text = inputPIN;
                }
            }
            else // angka 0–9
            {
                if (inputPIN.Length < 4)
                {
                    inputPIN += key;
                    displayText.text = inputPIN;
                }
            }
        }
    }
}