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
        if (wireManager.Done) return;

        CableEnds[] ports = FindObjectsOfType<CableEnds>();
        foreach (var port in ports) {
            if (port.cableSprite == sprite && !port.isConnected) {
                float dist = Vector3.Distance(cursorPos, port.rectTransform.position);
                if (dist < 50f) {
                    port.isConnected = true;
                    endPoint = port.rectTransform;
                    UpdateCable(startPoint.position, endPoint.position);
                    wireManager.WireConnected();
                    return;
                }
            }
        }

        // gagal snap → balik ke awal
        if (startPoint != null) {
            UpdateCable(startPoint.position, startPoint.position);
        }
    }

    // Reset kabel ke posisi awal
    public void ResetCable() {
        if (endPoint != null) {
            CableEnds port = endPoint.GetComponent<CableEnds>();
            if (port != null) {
                port.isConnected = false;
            }
        }

        endPoint = null;

        if (startPoint != null) {
            UpdateCable(startPoint.position, startPoint.position);
        } else {
            Debug.LogWarning("ResetCable dipanggil tapi startPoint masih null!");
        }
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