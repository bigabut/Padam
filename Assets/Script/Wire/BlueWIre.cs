 using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer), typeof(Collider2D))]
public class Wire : MonoBehaviour
{
    Camera cam;
    LineRenderer line;

    Vector3 startPos;
    bool dragging;
    bool connected;

    [Header("Snap")]
    public float snapRadius = 0.5f;
    public string targetTag = "blueTarget";

    void Awake()
    {
        cam = Camera.main;
        startPos = transform.position;

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startWidth = line.endWidth = 0.5f;
        line.startColor = line.endColor = Color.blue;

        line.SetPosition(0, startPos);
        line.SetPosition(1, startPos);
    }

    void Update()
    {
        if (connected) {

            return;
        }

        Vector2 screenPos = GetPointerPos();
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPos);
        worldPos.z = startPos.z;

        if (Pressed() && HitSelf(worldPos))
            dragging = true;

        if (dragging)
            line.SetPosition(1, worldPos);

        if (Released() && dragging)
        {
            dragging = false;
            TrySnap(worldPos);
        }

        line.SetPosition(0, startPos);
    }


    Vector2 GetPointerPos()
    {
        if (Mouse.current != null) return Mouse.current.position.ReadValue();
        if (Touchscreen.current != null) return Touchscreen.current.primaryTouch.position.ReadValue();
        return Vector2.zero;
    }

    bool Pressed()
    {
        if (Mouse.current != null) return Mouse.current.leftButton.wasPressedThisFrame;
        if (Touchscreen.current != null) return Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        return false;
    }

    bool Released()
    {
        if (Mouse.current != null) return Mouse.current.leftButton.wasReleasedThisFrame;
        if (Touchscreen.current != null) return Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
        return false;
    }

    bool HitSelf(Vector3 pos)
    {
        var col = Physics2D.OverlapPoint(pos);
        return col && col.gameObject == gameObject;
    }

    void TrySnap(Vector3 pos)
    {
        var hit = Physics2D.OverlapCircle(pos, snapRadius);

        if (hit && hit.CompareTag(targetTag))
        {
            line.SetPosition(1, hit.transform.position);

            var sr = hit.GetComponent<SpriteRenderer>();
            if (sr)
                line.startColor = line.endColor = sr.color;

            connected = true;
                    FindObjectOfType<WireManager>().WireConnected();

        }
        else
        {
            line.SetPosition(1, startPos);
        }
    }
}