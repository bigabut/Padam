 using UnityEngine;
using UnityEngine.UIElements;

public class Generator : MonoBehaviour
{
    [Header("References")]
    public UIDocument uiDocument;
    public TaskTimer taskTimer;

    [Header("Cursor")]
    public Texture2D hoverCursor;
    public Vector2 hotspot = Vector2.zero;

    [Header("Settings")]
    public float holdDuration = 3f;

    VisualElement container;
    VisualElement fill;

    float holdTimer;
    bool isHolding;
    bool taskActive;

    void Awake()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        if (uiDocument == null) return;

        var root = uiDocument.rootVisualElement;
        if (root == null) return;

        // Container untuk progress bar
        container = new VisualElement();
        container.style.width = 30;
        container.style.height = 200;
        container.style.backgroundColor = Color.gray;
        container.style.position = Position.Absolute;
        container.style.left = 400;
        container.style.bottom = 50;
        container.style.justifyContent = Justify.FlexEnd;

        // Fill (progress)
        fill = new VisualElement();
        fill.style.width = Length.Percent(100);
        fill.style.height = 0;
        fill.style.backgroundColor = Color.green;

        container.Add(fill);
        root.Add(container);

        container.visible = true;

        // Timer link
        if (taskTimer != null)
        {
            taskTimer.OnTimeUp += FailTask;
            taskTimer.StartTimer();
        }

        taskActive = true;
    }

    void Update()
    {
        if (!taskActive || fill == null) return;

        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            float percent = Mathf.Clamp01(holdTimer / holdDuration);
            fill.style.height = Length.Percent(percent * 100);

            if (holdTimer >= holdDuration)
                SuccessTask();
        }
    }

    void OnMouseDown()
    {
        if (!taskActive) return;
        holdTimer = 0f;
        isHolding = true;
    }

    void OnMouseUp()
    {
        if (!taskActive || fill == null) return;
        isHolding = false;
        holdTimer = 0f;
        fill.style.height = 0;
    }

    void OnMouseEnter()
    {
        UnityEngine.Cursor.SetCursor(hoverCursor, hotspot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void OnDisable()
    {
        if (taskTimer != null)
            taskTimer.OnTimeUp -= FailTask;
    }

    void SuccessTask()
    {
        taskActive = false;
        isHolding = false;

        if (taskTimer != null)
            taskTimer.StopTimer();

        if (container != null)
            container.visible = false;

        Debug.Log("TASK SUCCESS");
    }

    void FailTask()
    {
        taskActive = false;
        isHolding = false;

        if (container != null)
            container.visible = false;

        Debug.Log("TASK FAILED - TIME UP");
    }
}