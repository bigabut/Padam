using UnityEngine;

public class ButtonPaperHint : MonoBehaviour
{
    [SerializeField] GameObject targetUI; 
    public PlayerInteract playerInteract;

    public void HideUI()
    {
        if (targetUI != null)
            targetUI.SetActive(false);
            Time.timeScale = 1; 

    }
}