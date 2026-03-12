using UnityEngine;
using UnityEngine.EventSystems;

public class CableEnds : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public RectTransform rectTransform;
    public CableLines cableLine;
    public Sprite cableSprite;
    public WireManager wireManager;
    

    public bool isConnected = false; // flag port sudah dipakai

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!wireManager.Done && !isConnected) {
            cableLine.SetStart(rectTransform, cableSprite);
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (!wireManager.Done && !isConnected) {
            cableLine.SetTempEnd(eventData.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!wireManager.Done && !isConnected) {
            cableLine.TrySnapToPort(cableSprite, eventData.position);
        }
    }
}