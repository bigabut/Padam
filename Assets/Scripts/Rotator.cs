using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Transform aimTransform;
    private SpriteRenderer spriteRenderer;

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    // Get mouse position in world space with Z = 0
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // ubah -180..180 menjadi 0..360
        if (angle < 0)
            angle += 360;

        // rotasi flashlight
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        // ganti sprite berdasarkan sektor
        if (angle >= 315 || angle < 45)
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (angle >= 45 && angle < 135)
        {
            spriteRenderer.sprite = spriteDown;
        }
        else if (angle >= 135 && angle < 225)
        {
            spriteRenderer.sprite = spriteLeft;
        }
        else
        {
            spriteRenderer.sprite = spriteUp;
        }
    }
}