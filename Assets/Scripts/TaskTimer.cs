using UnityEngine;
using UnityEngine.UIElements;
using System;

public class TaskTimer : MonoBehaviour
{
    [Header("References")]
    public UIDocument uiDocument;
    public float duration = 15f;

    public Action OnTimeUp;

    Label timerLabel;
    VisualElement timerContainer;
    float timer;
    bool isRunning;
    bool initialized;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (initialized) return;
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;
        if (root == null) return;

        // Container untuk label
        timerContainer = new VisualElement();
        timerContainer.style.width = 80;
        timerContainer.style.height = 40;
        timerContainer.style.position = Position.Absolute;
        timerContainer.style.left = 10;
        timerContainer.style.top = 10;
        timerContainer.style.backgroundColor = Color.black;
        timerContainer.style.justifyContent = Justify.Center;
        timerContainer.style.alignItems = Align.Center;

        root.Add(timerContainer);

        // Label timer
        timerLabel = new Label("0");
        timerLabel.style.flexGrow = 1;
        timerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        timerLabel.style.color = Color.white;

        timerContainer.Add(timerLabel);
        timerLabel.visible = false;

        initialized = true;
    }

    void Update()
    {
        if (!initialized)
            Initialize();

        if (!isRunning || timerLabel == null) return;

        timer += Time.deltaTime;
        float remaining = Mathf.Max(duration - timer, 0f);

        timerLabel.text = Mathf.Ceil(remaining).ToString();

        if (timer >= duration)
        {
            StopTimer();
            OnTimeUp?.Invoke();
        }
    }

    public void StartTimer()
    {
        if (!initialized)
            Initialize();

        timer = 0f;
        isRunning = true;

        if (timerLabel != null)
            timerLabel.visible = true;
    }

    public void StopTimer()
    { 
        isRunning = false;

        if (timerLabel != null)
            timerLabel.visible = false;
    }
}