using UnityEngine;
using UnityEngine.UI;

public class CableLines : MonoBehaviour {
    public Image cableImage;
    private RectTransform startPoint;
    private RectTransform endPoint;

    public WireManager wireManager;

    public void SetStart(RectTransform start, Sprite sprite) {
        startPoint = start;
        cableImage.sprite = sprite;
    }

    public void SetTempEnd(Vector3 cursorPos) {
        if (!wireManager.Done) {
            UpdateCable(startPoint.position, cursorPos);
        }
    }

    public void TrySnapToPort(Sprite sprite, Vector3 cursorPos) {
        if (wireManager.Done) return; // stop kalau sudah selesai

        CableEnds[] ports = FindObjectsOfType<CableEnds>();
        foreach (var port in ports) {
            if (port.cableSprite == sprite && !port.isConnected) {
                float dist = Vector3.Distance(cursorPos, port.rectTransform.position);
                if (dist < 50f) {
                    port.isConnected = true; // tandai port sudah dipakai
                    endPoint = port.rectTransform;
                    UpdateCable(startPoint.position, endPoint.position);
                    wireManager.WireConnected();
                    return;
                }
            }
        }

        // kalau gagal snap → balik ke awal
        UpdateCable(startPoint.position, startPoint.position);
    }

    private void UpdateCable(Vector3 a, Vector3 b) {
        Vector3 dir = b - a;
        float distance = dir.magnitude;

        cableImage.rectTransform.position = a + dir * 0.5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cableImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        cableImage.rectTransform.sizeDelta = new Vector2(distance, cableImage.rectTransform.sizeDelta.y);
    }
}