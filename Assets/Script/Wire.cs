using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class WireStraight : MonoBehaviour
{
    private LineRenderer line;
    private Camera cam;
    private Vector3 startPos;
    private bool dragging = false;

    [Header("Snap Settings")]
    public float snapRadius = 0.5f;           // jarak maksimal snap
    public string targetTag = "blueTarget";   // tag object target

    void Awake()
    {
        cam = Camera.main;
        startPos = transform.position;

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;

        line.SetPosition(0, startPos);
        line.SetPosition(1, startPos);
    }

    void Update()
    {
        Vector2 pointerPos = Vector2.zero;
        bool pressed = false;
        bool released = false;

        // ===== ambil pointer =====
        if (Mouse.current != null)
        {
            pointerPos = Mouse.current.position.ReadValue();
            pressed = Mouse.current.leftButton.wasPressedThisFrame;
            released = Mouse.current.leftButton.wasReleasedThisFrame;
            if (!dragging && Mouse.current.leftButton.isPressed) pressed = false;
        }
        else if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            pointerPos = touch.position.ReadValue();
            pressed = touch.press.wasPressedThisFrame;
            released = touch.press.wasReleasedThisFrame;
        }

        Vector3 worldPos = cam.ScreenToWorldPoint(pointerPos);
        worldPos.z = 0;

        // ===== mulai drag =====
        if (pressed)
        {
            if (Physics2D.OverlapPoint(worldPos)?.transform == transform)
                dragging = true;
        }

        // ===== drag =====
        if (dragging)
            line.SetPosition(1, worldPos);

        // ===== lepas =====
        if (released)
        {
            dragging = false;

            // cek target snap
            Collider2D hit = Physics2D.OverlapCircle(worldPos, snapRadius);
            bool snapped = false;

            if (hit != null && hit.CompareTag(targetTag))
            {
                line.SetPosition(1, hit.transform.position);
                snapped = true;
            }

            if (!snapped)
                line.SetPosition(1, startPos);
        }

        // anchor selalu titik awal
        line.SetPosition(0, startPos);
    }
    void Drag(Vector2 screenPos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = 0; // pastikan z sama dengan LineRenderer / Camera setup

        line.SetPosition(0, startPos);   // anchor
        line.SetPosition(1, worldPos);   // ujung mengikuti cursor
    }
}