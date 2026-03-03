using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float elapsedTime = 15f;
    public bool isRunning = true;

    // Event untuk notifikasi
    public System.Action OnTimeUp;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime -= Time.deltaTime;

            if (elapsedTime <= 0)
            {
                elapsedTime = 0;
                isRunning = false;
                OnTimeUp?.Invoke(); // panggil event kalau waktu habis
            }
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}